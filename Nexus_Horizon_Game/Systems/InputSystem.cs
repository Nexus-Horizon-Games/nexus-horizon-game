using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus_Horizon_Game.Systems
{
    internal static class InputSystem
    {
        private struct InputAction
        {
            public Action<Keys> actionUp;
            public Action<Keys> actionDown;

            public bool isDown;

            public InputAction()
            {
                actionUp = (NA) => { };
                actionDown = (NA) => { };
                isDown = false;
            }
        }

        static private Dictionary<Keys, InputAction> keyActions = new Dictionary<Keys, InputAction>();

        public static void AddOnKeyDownListener(Keys key, Action<Keys> listener)
        {
            if (!keyActions.TryGetValue(key, out InputAction inputAction))
            {
                inputAction = new InputAction();
                keyActions.Add(key, inputAction);
            }

            inputAction.actionDown += listener;
            keyActions[key] = inputAction;
        }

        public static void RemoveOnKeyDownListener(Keys key, Action<Keys> listener)
        {
            if (!keyActions.TryGetValue(key, out InputAction inputAction))
            {
                throw new MissingFieldException("You Tried To Remove A Listener From A KeyDown InputAction That Does Not Exist");
            }

            inputAction.actionDown -= listener;
            keyActions[key] = inputAction;
        }

        public static void AddOnKeyUpListener(Keys key, Action<Keys> listener)
        {
            if (!keyActions.TryGetValue(key, out InputAction inputAction))
            {
                inputAction = new InputAction();
                keyActions.Add(key, inputAction);
            }

            inputAction.actionUp += listener;
            keyActions[key] = inputAction;
        }

        public static void RemoveOnKeyUpListener(Keys key, Action<Keys> listener)
        {
            if (!keyActions.TryGetValue(key, out InputAction inputAction))
            {
                throw new MissingFieldException("You Tried To Remove A Listener From A KeyUp InputAction That Does Not Exist");
            }

            inputAction.actionUp -= listener;
            keyActions[key] = inputAction;
        }

        public static bool IsKeyDown(Keys key)
        {
            return Keyboard.GetState().IsKeyDown(key);

            /*
            if (!keyActions.TryGetValue(key, out InputAction inputAction))
            {
                inputAction = new InputAction();
                keyActions.Add(key, inputAction);

                if (Keyboard.GetState().IsKeyDown(key))
                {
                    inputAction.isDown = true;
                }

                keyActions[key] = inputAction;
            }

            return keyActions[key].isDown;
            */
        }

        public static bool IsKeyUp(Keys key)
        {
            return Keyboard.GetState().IsKeyUp(key);
        }

        public static void Update()
        {
            KeysUpdate();
        }

        private static void KeysUpdate()
        {
            foreach (Keys key in keyActions.Keys)
            {
                if (Keyboard.GetState().IsKeyDown(key))
                {
                    InputAction inputAction = keyActions[key];
                    if (!inputAction.isDown)
                    {
                        inputAction.isDown = true;
                        keyActions[key] = inputAction;
                        inputAction.actionDown.Invoke(key);
                    }
                }
            }

            foreach (Keys key in keyActions.Keys)
            {
                if (Keyboard.GetState().IsKeyUp(key))
                {
                    InputAction inputAction = keyActions[key];
                    if (inputAction.isDown)
                    {
                        inputAction.isDown = false;
                        keyActions[key] = inputAction;
                        inputAction.actionUp.Invoke(key);
                    }
                }
            }
        }
    }
}
