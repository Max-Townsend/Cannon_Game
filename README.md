# Cannon Game

I built Cannon Game to study how people change their aiming strategy when visual feedback is altered. This repository contains the Unity/C# task used in Experiment 2b of Townsend et al. (2026), *Current Biology*.

Participants aim at eight target positions. The cannon also makes automatic adjustments that simulate implicit adaptation alongside the participant's strategic re-aiming. 

## Run the task

1. Clone this repository and open it in **Unity 2019.4.4f1**.
2. Open `Assets/Scenes/SampleScene.unity` and press **Play**.
3. Follow the instructions: **A / D** to aim, **Space** to fire.

Data are saved locally; AWS is optional. For browser builds, install Unity's WebGL Build Support module and choose **Tools → Cannon → Build WebGL**. See the [running guide](docs/RUNNING.md) for serving the build, exporting data and configuring AWS.

There is also a [browser demo](https://live0.d18azccbguoaon.amplifyapp.com/) of a different Cannon Game variant.

## Code

- [canonRotation.cs](Assets/canonRotation.cs): player input, instructions and the adaptation model.
- [blockTrial.cs](Assets/blockTrial.cs): target order, perturbations and trial progression.
- [ExperimentController.cs](Assets/ExperimentController.cs): trial recording and saving.
- [cannon-storage.js.txt](Assets/Editor/WebGLDependencies/cannon-storage.js.txt): browser storage and JSON export.

## Citation

Townsend, M., Warburton, M., Campagnoli, C., Mon-Williams, M., Mushtaq, F., & Morehead, J. R. (2026). [An Aha moment precedes the strategic response to a visuomotor rotation](https://doi.org/10.1016/j.cub.2026.04.021). *Current Biology, 36*(10), 2568–2580.e5. [Citation file](CITATION.cff).

Max Townsend · [max.o.b.townsend@gmail.com](mailto:max.o.b.townsend@gmail.com)
