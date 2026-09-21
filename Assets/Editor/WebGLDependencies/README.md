# WebGL dependencies

These dependencies support the Implicit8Tar task used in **Experiment 2b** of
Townsend et al. (2026). See the [project citation](../../../README.md#citation)
for the associated paper and author attribution.

The WebGL build hook loads these scripts before Unity starts:

- AWS SDK v2 2.1693.0 (Apache-2.0; see LICENSE and NOTICE files).
- Optional AWS configuration (disabled by default).
- Local storage, save status, and JSON export controls.

[AWS SDK source](https://github.com/aws/aws-sdk-js/tree/v2.1693.0).

The `.txt` suffix keeps Unity from interpreting JavaScript as UnityScript.
The build hook removes that suffix when copying files into a WebGL build.
See the [project README](../../../README.md) for setup and AWS configuration.
