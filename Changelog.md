### v2.3 (19-Dec-2020)

- Migrated to .NET Core 3.1.
- Fixed a few minor issues.

Note: This is the final release of osu!helper. No future development is expected.

### v2.2 (30-Nov-2019)

- Migrated to .NET Core 3.0. You will need to install .NET Core runtime in order to run this application starting from this version. You can download it [here](https://dotnet.microsoft.com/download/dotnet-core/3.0/runtime).
- Fixed an issue where map traits were incorrectly calculated with doubletime and halftime mods.

### v2.1.1 (17-Jul-2019)

- Updated the URL that opens when you click "Generate API key" in the settings dialog.
- Updated visual style of scrollbars.

### v2.1 (07-May-2019)

- Improved button layout on the beatmap details popup. Removed "close" button, dialog now closes on click away. Removed "open page" button, clicking beatmap title will now open its page. Pulled alternative download options from context menu to separate buttons, for easier access.
- Updated some links to point to the new osu! website.

### v2.0.3 (11-Apr-2019)

- Improved render quality of beatmap images.
- Current progress is now also shown in the taskbar.
- Fixed an issue where popup dialog would sometimes appear over other windows.

### v2.0.2 (15-Apr-2018)

- Implemented application auto-update (can be disabled in settings).

### v2.0.1 (31-Dec-2017)

- Fixed a crash when trying to preview a beatmap that doesn't allow previewing for non-logged in users.
- Fixed occasional crashes due to osu! API not responding properly under high request rates.

### v2.0 (30-Dec-2017)

- **The entire application was re-developed from scratch.**
- Reworked the UI to provide a more streamlined experience.
- Stopped using `oppai` for PP and map difficulty calculation.
- Increased size of data batches processed by recommendation service.
- Improved recommendation logic and its performance.
- Added some minor new features.
- Fixed numerous bugs.