#LolisBuddy


LolisBuddy aims to turn your VPet into a self learning AI.

Currently supports custom animation dialogues, sounds and basic memory.

Features / Roadmap:

✅ Customize Any Animation – Attach unique dialogue and sound effects to every action.

✅ Basic intelligence - Early framework for machine learning.

❌ ChatGPT Integration - Optional toggle to let user generate dialogue through AI.

❌ Self learning AI - Local Neural Network playing the game by itself and studying your behaviors.

📢 How to edit / add dialogue or sounds:

1️⃣ Locate the Files:

Go to: LolisBuddy/plugin/text

Open the .lps file corresponding to the animation type you want to modify.

*If you don't know the animation type / name / mood, you can find out by enabling debug in the mods settings*

2️⃣ Edit the Dialogue & Sound:

Example entry:

default:|Type#default:|Name#default:|Mood#Happy:|Dialogue#I'm having so much fun~!:|SoundEffect#test.wav:|

Syntax format:

type:|Type#type:|Name#name:|Mood#mood:|Dialogue#your_text_here:|SoundEffect#your_sound.wav:|

3️⃣ (Optional) Add Custom Sounds:

Place your new .wav files in: LolisBuddy/plugin/sound
