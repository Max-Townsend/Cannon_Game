# Implicit8Tar — Experiment 2b

This project contains the Unity cannon-aiming task used in **Experiment 2b** of
[An Aha moment precedes the strategic response to a visuomotor rotation](https://doi.org/10.1016/j.cub.2026.04.021)
(Townsend et al., 2026, *Current Biology*). **Max Townsend** is the paper's first
author and developed the task software.

Experiment 2b uses eight target positions and a cannon autocorrection model that
simulates implicit adaptation alongside participants' strategic re-aiming. See the
paper for the full experimental methods and results.

This public version supports local trial-data saving. AWS saving is optional and
disabled by default; setup and data-export instructions are below.

## Citation

If you use this task in research, please cite:

> Townsend, M., Warburton, M., Campagnoli, C., Mon-Williams, M., Mushtaq, F., &
> Morehead, J. R. (2026). An Aha moment precedes the strategic response to a
> visuomotor rotation. *Current Biology, 36*(10), 2568–2580.e5.
> https://doi.org/10.1016/j.cub.2026.04.021

[Publisher full text](https://www.cell.com/current-biology/fulltext/S0960-9822%2826%2900456-2)
· [Machine-readable citation](CITATION.cff)

## Run

1. Open the project in **Unity 2019.4.4f1**.
2. Open `Assets/Scenes/SampleScene.unity` and press **Play**.
3. Follow the on-screen instructions. Use **A/D** to aim and **Space** to fire.
   Browser trials require fullscreen.

To build for the browser, install Unity's **WebGL Build Support** module and choose
**Tools → Cannon → Build WebGL**. Serve `Builds/WebGL` over HTTP or use Unity's
**Build and Run**. The build hook includes the storage scripts automatically.

## Data

- **Browser:** Trials are saved to IndexedDB, with localStorage as a fallback.
  Use **Download saved data** to export JSON. Exit fullscreen if the button is
  hidden. Records include trial fields, session ID, timestamp, and remote-save
  status.
- **Editor / desktop:** Trials are appended to
  `Application.persistentDataPath/LocalData/cannon-data-<session>.jsonl`.
  The game displays the full path and save status.

Browser records belong to the current browser profile and site origin. Download
them after each session: clearing browser storage can remove them. If storage is
unavailable, the status warns that records are **only in memory**; download them
before closing the page.

## Optional AWS saving

WebGL builds support Cognito temporary credentials and DynamoDB through the bundled
AWS SDK v2.

1. Configure a Cognito identity pool and restrict its IAM role to the required
   DynamoDB tables.
2. Copy `Assets/Editor/WebGLDependencies/aws-config.example.js.txt` to
   `aws-config.local.js.txt` in the same directory.
3. Set `enabled: true`, `region`, `identityPoolId`, and the table mappings. For
   authenticated identities, provide `logins` tokens at runtime.
4. Rebuild. The local configuration is ignored by Git but included in the build.

Browser configuration is public: use temporary credentials, never long-lived
access keys. Every remote write retains a local copy. Failed remote writes appear
in the status and exports and are not retried automatically.
