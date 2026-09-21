# Running Cannon Game

[Back to the project overview](../README.md)

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
