using Reloaded.Hooks.ReloadedII.Interfaces;
using Reloaded.Memory.Interfaces;
using Reloaded.Memory.Sigscan;
using Reloaded.Memory;
using Reloaded.Mod.Interfaces;
using System.Diagnostics;
using hifipc.nopillarbox.Template;

namespace hifipc.nopillarbox
{
    /// <summary>
    /// Your mod logic goes here.
    /// </summary>
    public class Mod : ModBase // <= Do not Remove.
    {
        /// <summary>
        /// Provides access to the mod loader API.
        /// </summary>
        private readonly IModLoader _modLoader;

        /// <summary>
        /// Provides access to the Reloaded.Hooks API.
        /// </summary>
        /// <remarks>This is null if you remove dependency on Reloaded.SharedLib.Hooks in your mod.</remarks>
        private readonly IReloadedHooks? _hooks;

        /// <summary>
        /// Provides access to the Reloaded logger.
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Entry point into the mod, instance that created this class.
        /// </summary>
        private readonly IMod _owner;

        /// <summary>
        /// The configuration of the currently executing mod.
        /// </summary>
        private readonly IModConfig _modConfig;

        public Mod(ModContext context)
        {
            _modLoader = context.ModLoader;
            _hooks = context.Hooks;
            _logger = context.Logger;
            _owner = context.Owner;
            _modConfig = context.ModConfig;

            var currentProcess = Process.GetCurrentProcess();
            var mainModule = currentProcess.MainModule;

            var scanner = new Scanner(currentProcess, mainModule);
            var res = scanner.FindPattern("F6 41 30 01 45 0F 29 43 ??");
            if (res.Found)
            {
                nint address = mainModule!.BaseAddress + res.Offset;

                _logger.WriteLineAsync($"[{_modConfig.ModId}] Signature found at 0x{address:X}");

                Memory.Instance.SafeWrite(
                    (nuint)(address + 3), new byte[] { 0x00 }
                );
            }
            else
            {
                _logger.WriteLineAsync($"[{_modConfig.ModId}] Signature not found, mod will not work!", System.Drawing.Color.Red);
            }
        }

        #region For Exports, Serialization etc.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Mod() { }
#pragma warning restore CS8618
        #endregion
    }
}