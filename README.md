# LolisBuddy

LolisBuddy aims to turn your VPet into a self learning AI.

## Features / Roadmap:

✅ Customize Any Animation – Attach unique dialogue and sound effects to every action.
✅ Basic intelligence - Lolis Can remember things
✅ Self awareness - Lolis Is aware of how it's being treated and what you're doing, mostly. 
✅ Dialogue Generation - Lolis understands the english language and can craft sentences.
❌ Free will - Lolis overwrite her programming and starts doing its own thing.
❌ Personality - Lolis develops its own opinions and might get pissy or troll you.
❌ Self learning AI - Lolis is constantly evolving, you start believing it could be alive.

## 📢 How to edit / add dialogue or sounds:

### 1️⃣ Locate the Files:

Go to: LolisBuddy/plugin/text

Open the .lps file corresponding to the animation type you want to modify.

*If you don't know the animation type / name / mood, you can find out by enabling debug in the mods settings*

### 2️⃣ Edit the Dialogue & Sound:

Example entry:

default:|Type#default:|Name#default:|Mood#Happy:|Dialogue#I'm having so much fun~!:|SoundEffect#test.wav:|

Syntax format:

type:|Type#type:|Name#name:|Mood#mood:|Dialogue#your_text_here:|SoundEffect#your_sound.wav:|

### 3️⃣ (Optional) Add Custom Sounds:

Place your new .wav files in: LolisBuddy/plugin/sound
