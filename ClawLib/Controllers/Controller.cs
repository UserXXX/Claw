using Claw.Controllers.Controls;
using Claw.Documents;
using Claw.Validation;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Claw.Controllers
{
    /// <summary>
    /// Represents a controller (group). This is a kind/virtual group of controllers rather than a special controller (=hardware).
    /// The whole group uses the same control mapping. I.e. Mad Catz R.A.T. 7 and 9.
    /// </summary>
    public class Controller : NodeParser
    {
        private const string GROUP_ATTRIBUTE = "group";
        
        private const string MEMBER_CHILD_NODE = "member";
        private const string CONTROLS_CHILD_NODE = "controls";
        private const string SHIFTS_CHILD_NODE = "shifts";
        
        private const string MOUSE_POINTER_CHILD_NODE = "mousepointer";
        private const string MOUSE_AXIS_CHILD_NODE = "mouseaxis";
        private const string BUTTON_CHILD_NODE = "button";
        private const string SLIDER_CHILD_NODE = "slider";
        
        #region Check Stuff

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
        
        private Guid uuid;
        private DeviceGroup group;
        private LinkedList<Member> members = new LinkedList<Member>();
        private LinkedList<Control> controls = new LinkedList<Control>();
        private LinkedList<Shift> shifts = new LinkedList<Shift>();

        /// <summary>
        /// Creates a new controller from the given node.
        /// </summary>
        /// <param name="validator">The validator to use for validation.</param>
        /// <param name="node">The controller node.</param>
        internal Controller(NodeValidator validator, Node node)
        	: base(validator, node)
        {
            uuid = new Guid(node.Tag);
            group = DeviceGroupHelper.TryParse(node.Attributes[GROUP_ATTRIBUTE]);

            bool loadedControls = false;

            foreach (var child in node.Children)
            {
                switch (child.Name.ToLower())
                {
                    case MEMBER_CHILD_NODE:
                        members.AddLast(new Member(validator, child));
                        break;

                    case CONTROLS_CHILD_NODE:
                        if (loadedControls)
                            Trace.WriteLine("Found multiple \"controls\" nodes in \"controller\" node.");
                        else
                        {
                            LoadControls(validator, child);
                            loadedControls = true;
                        }
                        break;

                    case SHIFTS_CHILD_NODE:
                        LoadShifts(validator, child);
                        break;
                }
            }
        }

        /// <summary>
        /// Loads the shifts from a given node.
        /// </summary>
        /// <param name="validator">The validator to use for validation.</param>
        /// <param name="node">The "shifts" node.</param>
        private void LoadShifts(NodeValidator validator, Node node)
        {
            foreach (var child in node.Children)
                shifts.AddLast(new Shift(validator, child));
        }

        /// <summary>
        /// Loads the controls from a given Node.
        /// </summary>
        /// <param name="validator">The validator to use for validation.</param>
        /// <param name="node">The "controls" node.</param>
        private void LoadControls(NodeValidator validator, Node node)
        {
            foreach (var child in node.Children)
            {
                switch (child.Name.ToLower())
                {
                    case MOUSE_POINTER_CHILD_NODE:
                        controls.AddLast(new MousePointerControl(validator, child));
                        break;

                    case MOUSE_AXIS_CHILD_NODE:
                        controls.AddLast(new MouseAxisControl(validator, child));
                        break;

                    case BUTTON_CHILD_NODE:
                        controls.AddLast(new ButtonControl(validator, child));
                        break;

                    case SLIDER_CHILD_NODE:
                        controls.AddLast(new SliderControl(validator, child));
                        break;
                }
            }
        }
    }
}
