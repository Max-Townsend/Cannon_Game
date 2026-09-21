# Cannon Game

**An interactive experiment for studying how people discover and adapt movement strategies.**

Participants aim a cannon at targets while the relationship between their aim and visual feedback changes. This Unity project contains the eight-target task used in **Experiment 2b** of Townsend et al. (2026), *Current Biology*. A cannon autocorrection model simulates implicit adaptation alongside the participant's strategic re-aiming.

Developed by **Max Townsend** as part of his PhD research in computational cognitive science.

[Read the paper](https://doi.org/10.1016/j.cub.2026.04.021) · [Related browser demo](https://live0.d18azccbguoaon.amplifyapp.com/) · [Contact](mailto:max.o.b.townsend@gmail.com)

The hosted demo is a separate Cannon Game build; this repository contains the Experiment 2b variant.

## What this project demonstrates

- **Experimental software:** instructions, practice, trial blocks, eight target positions and controlled visual perturbations.
- **Behavioural measurement:** records key presses, timestamps, aiming angles, target position, feedback and trial outcomes.
- **Data collection across platforms:** C# task logic, a JavaScript bridge for WebGL, local data export and optional AWS saving.

**Stack:** Unity 2019.4.4f1 · C# · JavaScript · WebGL · AWS Cognito / DynamoDB (optional).

## Explore the code

| Start here | What to look for |
|---|---|
| [Task controller](Assets/canonRotation.cs) | Player input, instructions and the simulated adaptation model |
| [Trial sequence](Assets/blockTrial.cs) | Targets, perturbations, feedback and trial transitions |
| [Trial recording](Assets/ExperimentController.cs) | Behavioural fields and platform-specific saving |
| [Browser storage](Assets/Editor/WebGLDependencies/cannon-storage.js.txt) | Local persistence, save status and JSON export |

## Run locally

1. Clone this repository and open it in **Unity 2019.4.4f1**.
2. Open `Assets/Scenes/SampleScene.unity` and press **Play**.
3. Follow the instructions: **A / D** to aim, **Space** to fire.

Local saving works without AWS configuration. To build for a browser, install Unity's **WebGL Build Support** module, choose **Tools → Cannon → Build WebGL**, and serve `Builds/WebGL` over HTTP. Browser trials require fullscreen.

See [running, data export and AWS setup](docs/RUNNING.md) for storage locations, browser export instructions and configuration.

## Citation

Townsend, M., Warburton, M., Campagnoli, C., Mon-Williams, M., Mushtaq, F., & Morehead, J. R. (2026). **An Aha moment precedes the strategic response to a visuomotor rotation.** *Current Biology, 36*(10), 2568–2580.e5. [Paper and methods](https://doi.org/10.1016/j.cub.2026.04.021) · [Machine-readable citation](CITATION.cff).

**Questions about the task or research:** [max.o.b.townsend@gmail.com](mailto:max.o.b.townsend@gmail.com).
