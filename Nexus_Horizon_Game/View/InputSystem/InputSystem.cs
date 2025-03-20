using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nexus_Horizon_Game.View.InputSystem
{
    internal abstract class InputSystem
    {
        private struct InputAction
        {
            public Action actionUp;
            public Action actionDown;

            public bool isDown;

            public InputAction()
            {
                actionUp = () => { };
                actionDown = () => { };
                isDown = false;
            }
        }

        static private Dictionary<Keys, InputAction> keyActions = new Dictionary<Keys, InputAction>();

        static protected event Action OnUpdate;

        static private bool currentlyLoadingNewInput = false;

        private static void KeysPressedUpdate()
        {
            if (currentlyLoadingNewInput == true)
            {
                return;
            }

            Keys[] keys = keyActions.Keys.ToArray();

            foreach (Keys key in keys)
            {
                if (currentlyLoadingNewInput == true)
                {
                    return;
                }

                if (Keyboard.GetState().IsKeyDown(key))
                {
                    InputAction inputAction = keyActions[key];
                    if (!inputAction.isDown)
                    {
                        inputAction.isDown = true;
                        keyActions[key] = inputAction;
                        inputAction.actionDown.Invoke();
                        // keep the action down if loading new input on press.
                        if (currentlyLoadingNewInput == true)
                        {
                            if (keyActions.TryGetValue(key, out InputAction newInputAction))
                            {
                                newInputAction.isDown = true;
                                keyActions[key] = newInputAction;
                            }

                        }
                    }
                }
            }

            foreach (Keys key in keys)
            {
                if (currentlyLoadingNewInput == true)
                {
                    return;
                }

                if (Keyboard.GetState().IsKeyUp(key))
                {
                    InputAction inputAction = keyActions[key];
                    if (inputAction.isDown)
                    {
                        inputAction.isDown = false;
                        keyActions[key] = inputAction;
                        inputAction.actionUp.Invoke();
                    }
                }
            }
        }

        protected static void AddOnKeyDownListener(Keys key, Action listener)
        {
            if (!keyActions.TryGetValue(key, out InputAction inputAction))
            {
                inputAction = new InputAction();
                keyActions.Add(key, inputAction);
            }

            inputAction.actionDown += listener;
            keyActions[key] = inputAction;
        }

        protected static void RemoveOnKeyDownListener(Keys key, Action listener)
        {
            if (!keyActions.TryGetValue(key, out InputAction inputAction))
            {
                throw new MissingFieldException("You Tried To Remove A Listener From A KeyDown InputAction That Does Not Exist");
            }

            inputAction.actionDown -= listener;
            keyActions[key] = inputAction;
        }

        protected static void AddOnKeyUpListener(Keys key, Action listener)
        {
            if (!keyActions.TryGetValue(key, out InputAction inputAction))
            {
                inputAction = new InputAction();
                keyActions.Add(key, inputAction);
            }

            inputAction.actionUp += listener;
            keyActions[key] = inputAction;
        }

        protected static void RemoveOnKeyUpListener(Keys key, Action listener)
        {
            if (!keyActions.TryGetValue(key, out InputAction inputAction))
            {
                throw new MissingFieldException("You Tried To Remove A Listener From A KeyUp InputAction That Does Not Exist");
            }

            inputAction.actionUp -= listener;
            keyActions[key] = inputAction;
        }

        protected abstract void LoadInput();

        public static bool IsKeyDown(Keys key)
        {
            return Keyboard.GetState().IsKeyDown(key);
        }

        public static bool IsKeyUp(Keys key)
        {
            return Keyboard.GetState().IsKeyUp(key);
        }

        public static void SetInputSystem(InputSystem inputSystem)
        {
            InputSystem.currentlyLoadingNewInput = true;
            InputSystem.keyActions.Clear();
            InputSystem.OnUpdate = null;
            inputSystem.LoadInput();
        }

        public static void Update(GameTime gameTime)
        {
            currentlyLoadingNewInput = false;
            KeysPressedUpdate();
            OnUpdate?.Invoke();
        }
    }
}
