using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This code is part of the Fungus library (https://github.com/snozbot/fungus)
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)
using UnityEngine.EventSystems;

namespace Fungus
{
    /// <summary>
    /// Input handler for say dialogs.
    /// </summary>
    public class MyDialogInput : DialogInput
    {
        protected override void Update()
        {
            if (EventSystem.current == null)
            {
                return;
            }

            if (currentStandaloneInputModule == null)
            {
                currentStandaloneInputModule = EventSystem.current.GetComponent<StandaloneInputModule>();
            }

            if (writer != null)
            {
                if (GameManager.Instance.CommonInputAction.GetPerformedTypeThisFrame() == CommonInputAction.EType.Enter ||
                    (cancelEnabled && GameManager.Instance.CommonInputAction.GetPerformedTypeThisFrame() == CommonInputAction.EType.Cancel))
                {
                    SetNextLineFlag();
                }
            }

            switch (clickMode)
            {
                case ClickMode.Disabled:
                    break;
                case ClickMode.ClickAnywhere:
                    if (Input.GetMouseButtonDown(0))
                    {
                        SetClickAnywhereClickedFlag();
                    }
                    break;
                case ClickMode.ClickOnDialog:
                    if (dialogClickedFlag)
                    {
                        SetNextLineFlag();
                        dialogClickedFlag = false;
                    }
                    break;
            }

            if (ignoreClickTimer > 0f)
            {
                ignoreClickTimer = Mathf.Max(ignoreClickTimer - Time.deltaTime, 0f);
            }

            if (ignoreMenuClicks)
            {
                // Ignore input events if a Menu is being displayed
                if (MenuDialog.ActiveMenuDialog != null &&
                    MenuDialog.ActiveMenuDialog.IsActive() &&
                    MenuDialog.ActiveMenuDialog.DisplayedOptionsCount > 0)
                {
                    dialogClickedFlag = false;
                    nextLineInputFlag = false;
                }
            }

            // Tell any listeners to move to the next line
            if (nextLineInputFlag)
            {
                var inputListeners = gameObject.GetComponentsInChildren<IDialogInputListener>();
                for (int i = 0; i < inputListeners.Length; i++)
                {
                    var inputListener = inputListeners[i];
                    inputListener.OnNextLineEvent();
                }
                nextLineInputFlag = false;
            }
        }

    }
}
