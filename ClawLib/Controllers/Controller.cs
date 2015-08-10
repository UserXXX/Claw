using Claw.Controllers.Controls;
using Claw.Documents;
using Claw.Validation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Resources;

namespace Claw.Controllers
{
    /// <summary>
    /// Represents a controller (group). This is a kind/virtual group of controllers rather than a special controller (=hardware).
    /// The whole group uses the same control mapping. I.e. Mad Catz R.A.T. 7 and 9.
    /// </summary>
    public class Controller : NodeParser
    {
        private const string GROUP_ATTRIBUTE = "group";
        
        internal const string MEMBER_CHILD_NODE = "MEMBER";
        internal const string CONTROLS_CHILD_NODE = "CONTROLS";
        internal const string SHIFTS_CHILD_NODE = "SHIFTS";

        #region Validation

        private static readonly string[] REQUIRED_ATTRIBUTES = {
            GROUP_ATTRIBUTE,
        };
        private static readonly string[] OPTIONAL_ATTRIBUTES = { };
        private static readonly string[] REQUIRED_CHILD_NODES = { };
        private static readonly string[] OPTIONAL_CHILD_NODES = {
        	MEMBER_CHILD_NODE,
        	CONTROLS_CHILD_NODE,
        	SHIFTS_CHILD_NODE,
        };

        internal override string[] RequiredAttributes
        {
            get { return REQUIRED_ATTRIBUTES; }
        }

        internal override string[] OptionalAttributes
        {
            get { return OPTIONAL_ATTRIBUTES; }
        }

        internal override string[] RequiredChildNodes
        {
            get { return REQUIRED_CHILD_NODES; }
        }

        internal override string[] OptionalChildNodes
        {
            get { return OPTIONAL_CHILD_NODES; }
        }
        
		internal override TagUsage TagUsageType
		{
			get { return TagUsage.Required; }
		}

        #endregion

        /// <summary>
        /// Unique identifier for the R.A.T. mice.
        /// </summary>
        public static readonly Guid RatMiceIdentifier = new Guid("c9e4beb7-9967-4ce8-8fbc-02ca04f453d8");
        /// <summary>
        /// Unique identifier for the Strike 7 keyboards.
        /// </summary>
        public static readonly Guid Strike7Identifier = new Guid("7dd18c88-ccc8-4fe7-ae24-17fcb414aa53");
        /// <summary>
        /// Unique identifier for the Strike 5 keyboards.
        /// </summary>
        public static readonly Guid Strike5Identifier = new Guid("4ae960ae-0df5-4cf5-8d9a-f90a660afa73");
        /// <summary>
        /// Unique identifier for the R.A.T.M mice.
        /// </summary>
        public static readonly Guid RatMIdentifier = new Guid("6ad0bb35-7395-435c-a710-63ffb566aa0c");
        /// <summary>
        /// Unique identifier for the M.O.U.S.9 mice.
        /// </summary>
        public static readonly Guid Mous9Identifier = new Guid("e3d5b5a4-8391-430d-b538-0041c6934bb5");

        /// <summary>
        /// All known identifiers.
        /// </summary>
        private static readonly List<Guid> KNOWN_IDENTIFIERS = new List<Guid>()
        {
            RatMiceIdentifier,
            Strike7Identifier,
            Strike5Identifier,
            RatMIdentifier,
            Mous9Identifier,
        };

        /// <summary>
        /// Mapping of identifiers (UUIDs) to the creation method.
        /// </summary>
        private static readonly Dictionary<Guid, Func<Controller>> IDENTIFIER_CREATE_MAPPING = new Dictionary<Guid, Func<Controller>>()
        {
            { RatMiceIdentifier, new Func<Controller>(CreateRATMouseController) },
            { Strike7Identifier, new Func<Controller>(CreateStrike7KeyboardController) },
            { Strike5Identifier, new Func<Controller>(CreateStrike5KeyboardController) },
            { RatMIdentifier, new Func<Controller>(CreateRATMMouseController) },
            { Mous9Identifier, new Func<Controller>(CreateMOUS9MouseController) },
        };

        private Guid uuid;
        private DeviceGroup group;
        /// <summary>
        /// List of members (=devices) of this group.
        /// Use LinkedList here as a Controller has members and the usage of this node is not to store members (alternate
        /// implementation would be inheriting from NodeListParser instead of NoeParser).
        /// </summary>
        private LinkedList<Member> members = new LinkedList<Member>();
        private ControlList controls;
        private ShiftList shifts;

        /// <summary>
        /// The unique identifier of this controller.
        /// </summary>
        public Guid Uuid
        {
            get { return uuid; }
        }

        /// <summary>
        /// List of members (=devices) of this group.
        /// </summary>
        public LinkedList<Member> Members
        {
            get { return members; }
        }

        /// <summary>
        /// The controls of this controller.
        /// </summary>
        public ControlList Controls
        {
            get { return controls; }
        }

        /// <summary>
        /// Gets the available shifts (called "modes" by MadCatz).
        /// </summary>
        public ShiftList Shifts
        {
            get { return shifts; }
        }

        /// <summary>
        /// Creates a new controller from the given node.
        /// </summary>
        /// <param name="validator">The validator to use for validation.</param>
        /// <param name="node">The controller node.</param>
        internal Controller(NodeValidator validator, Node node)
        	: base(validator, node)
        {
            if (!string.IsNullOrEmpty(node.Tag))
            {
                uuid = new Guid(node.Tag);
            }
            if (node.Attributes.ContainsKey(GROUP_ATTRIBUTE))
            {
                group = DeviceGroupHelper.TryParse(node.Attributes[GROUP_ATTRIBUTE]);
            }

            foreach (var child in node.Children)
            {
                switch (child.Name.ToUpperInvariant())
                {
                    case MEMBER_CHILD_NODE:
                        members.AddLast(new Member(validator, child));
                        break;

                    case CONTROLS_CHILD_NODE:
                        controls = new ControlList(validator, child);
                        break;

                    case SHIFTS_CHILD_NODE:
                        shifts = new ShiftList(validator, child);
                        break;
                }
            }
        }

        /// <summary>
        /// Checks whether the conttoller group is one of the known groups.
        /// </summary>
        /// <returns>Whether the controller group is one of the known groups.</returns>
        public bool IsKnown()
        {
            return KNOWN_IDENTIFIERS.Contains(uuid);
        }


        /// <summary>
        /// Creates the node structure.
        /// </summary>
        /// <returns>The node.</returns>
        internal Node CreateNodes()
        {
            var node = new Node(ControllerList.CONTROLLER_CHILD_NODE);
            if (uuid != null)
            {
                node.Tag = uuid.ToString();
            }
            node.Attributes.Add(GROUP_ATTRIBUTE, DeviceGroupHelper.ToString(group));
                      
            // *.pr0 files have the first member as the first node and the others listed at the end of the node list.
            if (members.Count > 0)
            {
                node.Children.AddLast(members.First.Value.CreateNodes());
            }

            node.Children.AddLast(controls.CreateNodes());
            node.Children.AddLast(shifts.CreateNodes());
            
            if (members.Count > 1)
            {
                LinkedList<Member>.Enumerator enumerator = members.GetEnumerator();
                enumerator.MoveNext();
                while (enumerator.MoveNext())
                {
                    node.Children.AddLast(enumerator.Current.CreateNodes());
                }
            }
            return node;
        }

        /// <summary>
        /// Creates the default controller node for R.A.T. mice.
        /// This group contains:
        /// - Call Of Duty: Black Ops - Stealth Mouse
        /// - Cyborg R.A.T. 5
        /// - Cyborg R.A.T.7
        /// - Contagion
        /// - Albino
        /// - Cyborg R.A.T. 5
        /// - Cyborg R.A.T.7
        /// - Cyborg R.A.T.9 (2 times with different identifiers)
        /// - Mad Catz R.A.T. 5
        /// - Mad Catz R.A.T.7
        /// - Mad Catz R.A.T.9
        /// </summary>
        /// <returns>The default controller node.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "RAT", Justification="This is the product identifier for R.A.T. mice.")]
        public static Controller CreateRATMouseController()
        {
            return LoadDefaultController("R.A.T.Controller");
        }

        /// <summary>
        /// Creates the default controller node for Strike 7 keyboards.
        /// This group contains:
        /// - Cyborg Strike 7
        /// - Mad Catz Strike 7
        /// </summary>
        /// <returns>The default controller node.</returns>
        public static Controller CreateStrike7KeyboardController()
        {
            return LoadDefaultController("Strike7Controller");
        }

        /// <summary>
        /// Creates the default controller node for Strike 5 keyboards.
        /// This group contains:
        /// - Mad Catz Strike 5
        /// </summary>
        /// <returns>The default controller node.</returns>
        public static Controller CreateStrike5KeyboardController()
        {
            return LoadDefaultController("Strike5Controller");
        }

        /// <summary>
        /// Creates the default controller node for R.A.T.M mice.
        /// This group contains:
        /// - R.A.T.M
        /// </summary>
        /// <returns>The default controller node.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "RATM", Justification="This is the product identifier for the R.A.T.M mouse.")]
        public static Controller CreateRATMMouseController()
        {
            return LoadDefaultController("R.A.T.MController");
        }

        /// <summary>
        /// Creates the default controller node for M.O.U.S.9 mice.
        /// This group contains:
        /// - M.O.U.S.9
        /// </summary>
        /// <returns>The default controller node.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "MOUS", Justification="This is the product identifier for the M.O.U.S.9 mouse.")]
        public static Controller CreateMOUS9MouseController()
        {
            return LoadDefaultController("M.O.U.S.9Controller");
        }

        /// <summary>
        /// Loads a default controller from a resource file embedded into this library.
        /// </summary>
        /// <param name="profileFile">The name of the profile file.</param>
        /// <returns>The loaded controller.</returns>
        private static Controller LoadDefaultController(string profileFile)
        {
            var baseNode = new Node(new PR0Reader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Claw.Controllers." + profileFile)), false);
            return new Controller(new NodeValidator(new EmptyReport()), baseNode);
        }

        /// <summary>
        /// Creates all default controller nodes for all known device classes.
        /// </summary>
        /// <returns>The created controllers-</returns>
        public static Controller[] CreateAllControllers()
        {
            return new Controller[] {
                CreateRATMouseController(),
                CreateStrike7KeyboardController(),
                CreateStrike5KeyboardController(),
                CreateRATMMouseController(),
                CreateMOUS9MouseController(),
            };
        }

        /// <summary>
        /// Creates the default controller for the given UUID. Throws an ArgumentException if the given UUID is not one of
        /// the default UUIDs.
        /// </summary>
        /// <param name="uuid">The UUID of the controller to create.</param>
        /// <returns>The created controller.</returns>
        public static Controller CreateDefaultById(Guid uuid)
        {
            if (!IDENTIFIER_CREATE_MAPPING.ContainsKey(uuid))
            {
                throw new ArgumentException("The requested UUID " + uuid.ToString() + " is unknwon.");
            }

            return IDENTIFIER_CREATE_MAPPING[uuid].Invoke();
        }
    }
}
