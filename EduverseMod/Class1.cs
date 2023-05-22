using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine;
using MelonLoader;
using TMPro;
using Invector.vCharacterController;
using UrGUI.GUIWindow;

namespace MeduverseMod
{
    // ReSharper disable UnusedMember.Global
    public class MeduverseHacking : MelonMod
    {
        public static bool FlyHack;
        public static bool SpeedHack;
        public static bool JumpHack;
        public static bool TimeHack;
        public static bool AutoTypeCertificate;
        public static bool AutoTypeContest;
        public static bool AntiCheat;
        
        public static float JumpHeight;
        public static float WalkSpeed;
        public static float GameTime;

        private GUIWindow _window;
        private bool _enableMenu;
        public override void OnInitializeMelon()
        {
            GUIBuilder();
        }
        public override void OnGUI()
        {
            if (_enableMenu)
            {
                _window.Draw();
            }
        }
        public override void OnUpdate()
        {
            _enableMenu = Input.GetKeyDown(KeyCode.RightShift) ? !_enableMenu : _enableMenu;
            if(TimeHack) Time.timeScale = GameTime;
        }
        private void GUIBuilder()
        {
            _window = GUIWindow.Begin("Cheat Menu");
            _window.Toggle("Flight", (value) => FlyHack = value);
            _window.Toggle("Auto Type Certificate", (value) => AutoTypeCertificate = value, true);
            _window.Toggle("Auto Type Contest", (value) => AutoTypeContest = value, true);
            _window.Toggle("Anti AntiCheat", (value) => AntiCheat = value, true);
            _window.Toggle("Speed hack", (value) => SpeedHack = value);
            _window.Toggle("Jump Boost", (value) => JumpHack = value);
            _window.Toggle("Enable Time Hack", (value) => TimeHack = value);

            _window.FloatField("Speed", (value) => WalkSpeed = value, 4f);
            _window.FloatField("Jump Height", (value) => JumpHeight = value, 4f);
            _window.FloatField("Time Machine",(value) => GameTime = value, 1f);
            // _window.FloatField("FloatField:", (value) => , speed);
            // _window.Label("Label");
            // _window.Button("Button!", () => Debug.Log("Button has been pressed!"));
            // _window.Slider("Slider:", (value) => Debug.Log($"Toggle value is now {value}"), 0.69f, 0f, 1f, true);
            // _window.ColorPicker("ColorPicker:", (clr) => Debug.Log($"Color has been changed to {clr}"), Color.red);
            // var selection = new Dictionary<int, string>();
            // for (int i = 0; i < 10; i++)
            //     selection.Add(i, $"Option n.{i}");
            // _window.DropDown("DropDown:", (id) => Debug.Log($"'{id}'. has been selected!"), 0, selection);
            // _window.TextField("TextField:", (value) => Debug.Log($"TextField has been changed to '{value}'"),
            //     "Sample Text");
            // _window.IntField("IntField:", (value) => Debug.Log($"IntField has been changed to '{value}'"), 123456);
            
        }
    }
    //Patch
    // ReSharper disable InconsistentNaming
    [HarmonyPatch(typeof(vThirdPersonInput), "JumpConditions")]
    public static class FlyPatch
    {
        public static bool Prefix(ref bool __result)
        {
            if (MeduverseHacking.FlyHack)
            {
                __result = true;
                return false;
            }
            return true;
        }
    }
    [HarmonyPatch(typeof(vThirdPersonMotor), "SetControllerMoveSpeed")]
    public static class SpeedPatch
    {
        public static void Prefix(ref vThirdPersonMotor.vMovementSpeed __0)
        {
            if (MeduverseHacking.SpeedHack)
            {
                __0.walkSpeed = MeduverseHacking.WalkSpeed;
                __0.runningSpeed = MeduverseHacking.WalkSpeed;
                __0.sprintSpeed = MeduverseHacking.WalkSpeed;
            }
            else
            {
                __0.walkSpeed = 2f;
                __0.runningSpeed = 4f;
                __0.sprintSpeed = 6f;
            }
        }
    }
    [HarmonyPatch(typeof(TypingGame), "OnClickShoot")]
    public static class TypingGamePatch
    {
        public static void Prefix(ref TMP_InputField ___ipfContent, String ____curWord)
        {
            if(MeduverseHacking.AutoTypeCertificate) ___ipfContent.text = ____curWord;
        }
    }
    [HarmonyPatch(typeof(MathBattleGame), "OnClickShoot")]
    public static class MathBattleGamePatch
    {
        public static void Prefix(ref TMP_InputField ___ipfContent, String ____curWord)
        {
            if(MeduverseHacking.AutoTypeContest) ___ipfContent.text = ____curWord;
        }
    }
    [HarmonyPatch(typeof(TypingGame), "SetStateButtonShoot")]
    public static class DisableSetStateButtonShoot
    {
        public static bool Prefix()
        {
            return false;
        }
    }
    [HarmonyPatch(typeof(vThirdPersonMotor), "ControlJumpBehaviour")]
    public static class JumpPatch
    {
        public static void Prefix(vThirdPersonMotor __instance)
        {
            __instance.jumpHeight = MeduverseHacking.JumpHack ? MeduverseHacking.JumpHeight : 4f;
        }
    }

    [HarmonyPatch(typeof(TypingGame), "EndCerfiticate")]
    public static class AntiCheatPatch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);
            if(MeduverseHacking.AntiCheat) for (var i = 23; i <= 43; i++) codes[i].opcode = OpCodes.Nop;
            return codes;
        }
    }
} 