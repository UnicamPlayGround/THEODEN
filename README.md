# THEODEN Framework
## Tool for Hot EnvirOnment upDate and modEl visualisatioN

[![Made with Django](https://img.shields.io/badge/Made%20with-Django-57b9d3?style=flat&logo=django)](https://www.djangoproject.com/)
[![Made with Unity](https://img.shields.io/badge/Made%20with-Unity-57b9d3?style=flat&logo=unity)](https://unity3d.com)
[![Unity 2021.3.25f1](https://img.shields.io/badge/Unity-2021.3.25f1-57b9d3?style=flat&logo=unity)](https://docs.unity3d.com/Manual/index.html)
[![License](https://img.shields.io/github/license/UnicamPlayGround/TEnCu)](/LICENSE)
[![Issues](https://img.shields.io/github/issues/UnicamPlayGround/TEnCu)](https://github.com/UnicamPlayGround/TEnCu/issues)
[![Pull Requests](https://img.shields.io/github/issues-pr/UnicamPlayGround/TEnCu)](https://github.com/UnicamPlayGround/TEnCu/pulls)
[![Contributors](https://img.shields.io/github/contributors/UnicamPlayGround/TEnCu)](https://github.com/UnicamPlayGround/TEnCu/graphs/contributors)
[![Forks](https://img.shields.io/github/forks/UnicamPlayGround/TEnCu?style=social)](https://github.com/UnicamPlayGround/TEnCu/fork)

<hr/>

# Intro
<!-- Brief description of the framework -->
Nowadays many applications show 3D models or 2D sprites. Is also common to let the user explore the environments or interact with these objects.
THEODEN: Tool for Hot EnvirOnment upDate and modEl visualisatioN, is a framework that aims to help the development of games and apps that have to show models and sprites.

# Full Documentation
See the [Wiki](https://github.com/UnicamPlayGround/TEnCu/wiki) for full documentation, examples, operational details and other information.

# Communication
- [Github Discussions](https://github.com/UnicamPlayGround/TEnCu/discussions)
- [Github Issues](https://github.com/UnicamPlayGround/TEnCu/issues)

# Features and Functionalities
<!-- Bullet list with core points and functionalities -->
- Model Exploration: The framework allows a model exploration and provides touch input support out of the box.
- Customisation: Camera and model settings depend on JSON files provided with the Unity asset bundles.
- Usability: Simple to change a model behaviour and to update the framework to fit the project needs.
- Scalability: Simple to change the number of models shown at runtime by using the server.
- Hot Update: The server presence allows runtime minor changes and updates to the models.
- Availability: An app implementing this framework will require fewer updates, thus, an application can be sustained for a longer time with lower effort.

# How to use
This is a brief description of how to use the framework, a more specified version is available in the [Wiki](https://github.com/UnicamPlayGround/TEnCu/wiki/How-to-Use)
1. Download the [latest release](https://github.com/UnicamPlayGround/TEnCu/releases)
2. Open your Unity Project
3. Import the [Unity package](/unity%20package/)
4. (Optional) Map the input method to the camera gestures
5. Build the Asset Bundles
6. If you want the model's hot update
   1. Use the provided server or create your server based on the provided one
   2. Set the server address
   3. Create or Update the Download Scene
7. Enjoy your app

# Bugs and Feedback
For bugs or to suggest new features, please use the [GitHub Issues](https://github.com/UnicamPlayGround/TEnCu/issues).
<br/>
For questions and discussions, please use the [Github Discussions](https://github.com/UnicamPlayGround/TEnCu/discussions).
