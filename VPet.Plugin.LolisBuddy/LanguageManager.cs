using System;
using System.Collections.Generic;
using System.Windows;

namespace VPet.Plugin.LolisBuddy
{
    public class LanguageManager
    {
        private readonly Random random = new Random();

        public DialogueEntry GenerateDialogue(ActiveWindow win, string aiMood)
        {
            aiMood = aiMood.ToLower();
            win.Runtime = Convert.ToInt32(Math.Round(win.Runtime / 60.0));

            List<string> dialogues = aiMood switch
            {
                "happy" => GetHappyResponses(win.Title, win.Runtime, DateTime.Parse(win.Date), win.Category),
                "nomal" => GetNormalResponses(win.Title, win.Runtime, DateTime.Parse(win.Date), win.Category),
                "ill" => GetIllResponses(win.Title, win.Runtime, DateTime.Parse(win.Date), win.Category),
                "poorcondition" => GetPoorConditionResponses(win.Title, win.Runtime, DateTime.Parse(win.Date), win.Category),
                _ => GetNormalResponses(win.Title, win.Runtime, DateTime.Parse(win.Date), win.Category),
            };

            if (dialogues.Count == 0)
            {
                MessageBox.Show($"Debug: No dialogues found for aiMood={aiMood}, category={win.Category}");
            }

            DialogueEntry entry = new()
            {
                Mood = aiMood,
                Dialogue = dialogues.Count > 0 ? dialogues[random.Next(dialogues.Count)] : "I don't know what to say..."
            };

            return entry;
        }


        private List<string> GetHappyResponses(string appName, int totalRuntime, DateTime lastUsedDate, string appCategory)
        {

            return appCategory switch
            {
                "Browser" => new() { $"Is this ‘research’ or just a very elaborate way to procrastinate?", $"Browsing {appName} again huh?" },
                "Game Launcher" => new() { $"You've spent {totalRuntime} hours in {appName}. That’s a lot of time… for deciding what to play.", $"Another gaming session? I’ll prepare the snacks. Oh wait, I live in your computer." },
                "Messaging App" => new() { $"Typing away, huh? Hope you’re not just leaving people on read.", $"Another conversation? You better not be ignoring me for them!" },
                "Development Tool" => new() { $"Coding again? You’re so dedicated! Or is it just debugging pain?", $"You type so fast! Are you coding or casting a spell?" },
                "Text Editor" => new() { $"Writing something cool? Can i have a look?", $"Are you writing me a poem? I’d totally deserve one.", $"Your fingers are moving, but is it genius or nonsense?" },
                "Image Editor" => new() { $"Ooooh, art time! Can I see when it’s done?", $"Creating something beautiful in {appName}? I love it!", $"Are we editing, or just aggressively undoing mistakes?", $"I’d love a portrait of myself. Just saying." },
                "3D Modeling Software" => new() { $"I hope you're working on me.", $"3D modeling, huh? Are we making art or another headache?", $"I bet whatever you’re making is gonna look awesome!" },
                "Rigging Tool" => new() { $"Rigging things in {appName} huh? I wish i could learn new moves.", $"Is that a new animation for me?", $"Just don’t mess up the weights, or things will get... weird." },
                "Game Engine" => new() { $"So, when do I get my own game?" },
                "Audio Editing Tool" => new() { $"Turn up the volume! Oh wait, I live in here...", $"Hope you're making something I can vibe to!" },
                "Music Player" => new() { $"If you start headbanging, I want a front-row seat.", $"Careful! Too much replay and you’ll ruin the song forever." },
                "Video Player" => new() { $"Movie time! Got snacks ready?", $"Is this a cinematic masterpiece or just another meme compilation?" },
                "Torrenting App" => new() { $"Is this legal? I won’t snitch, promise.", $"I hope you're using a private tracker." },
                "Streaming Software" => new() { $"Your mic is muted.", $"Chat, press 1 if i'm cool." },
                "Cloud Storage / Backup Service" => new() { $"Backing up files? You’re so responsible!", $"At least someone here is thinking ahead!" },
                "Virtual Machine / Emulator" => new() { $"One computer inside another… are you trying to start a simulation?", $"Virtual machines, huh? Are we hacking the Matrix today?" },
                "Benchmarking / Hardware Monitoring Tool" => new() { $"I see {appName} is open… worried about temps or just flexing specs on me?", $"Benchmarking? Trying to make me cry?" },
                "Download Manager" => new() { $"How many downloads do you actually finish?", $"Waiting for downloads is pain. I feel you." },
                "Password Manager" => new() { $"If I had a password, it’d be Lolisecret, but you’re smarter than that, right?", $"Another login? At least you don’t have to remember all those passwords." },
                "Code Compiler / Build Tool" => new() { $"Build success? Or did you just create a whole new set of problems?", $"Building something cool? Hope there are no errors!", $"Oh, {appName} is running. Time to see if it works or crashes spectacularly!" },
                "System Utility / Optimization Tool" => new() { $"Cleaning up? You better not throw me away by accident!", $"Oh wow, {appName}? Either you’re a responsible user or something’s on fire.", $"Deleting junk files? Maybe let go of some emotional baggage too." },
                "PDF Reader / Document Viewer" => new() { $"Skimming or actually reading?", $"Reading something interesting? Tell me about it!" },
                "CAD Software" => new() { $"CAD work, huh? Your brain must be huge.", $"So, are we designing a building or a futuristic spaceship?" },
                "Screenshot / Screen Annotation Tool" => new() { $"Screenshotting? Is it for work, or just another meme?", $"Snapping pics of your screen like a pro!" },
                "Note-Taking / Mind Mapping App" => new() { $"Filling up another document with ideas you’ll totally follow through on, right?", $"Keeping track of ideas? That’s so smart!" },
                "Image Viewer" => new() { $"Looking at images? Maybe it’s time to make some new ones!", $"Photo time! Anything cool?" },
                "VPN Client" => new() { $"Staying safe online? Smart choice!", $"Hiding in the digital shadows? Very sneaky~!" },
                "Antivirus" => new() { $"Keeping your system safe? I approve!", $"Running {appName}? Someone’s being extra careful today!" },
                "Game" => new() { $"Ooooh, gaming time! Let’s see if you actually win this time.", $"Try not to rage quit this time, okay?", $"You’re about to enter the gamer zone… don’t forget to hydrate!", $"Speedrun your homework like you speedrun these games, maybe?", $"If you win, I’ll celebrate. If you lose… well, I’ll still be here judging you." },
                _ => new() { $"Mysterious app detected… what exactly are you up to?", $"I have no idea what this app does, but I’ll pretend to be impressed!" }
            };
        }


        private List<string> GetNormalResponses(string appName, int totalRuntime, DateTime lastUsedDate, string appCategory)
        {
            return appCategory switch
            {
                "Browser" => new() { $"Still browsing {appName}? Must be important… or not.", $"Another deep dive into the internet? I bet you don’t even remember what you were looking for." },

                "Game Launcher" => new() { $"You've spent {totalRuntime} hours in {appName}, and yet, still can’t decide what to play.", $"Another gaming session? Hope launching the game doesn’t take longer than playing it." },

                "Messaging App" => new() { $"More typing? Bet half of it is just ‘lol’ and reaction emojis.", $"Another conversation? You’re either really social or really avoiding something." },

                "Development Tool" => new() { $"Coding again? More bugs or more suffering?", $"Oh look, more lines of code. Just what you needed, right?" },

                "Text Editor" => new() { $"Writing again? That document is probably 90% unfinished thoughts.", $"Is this an important document, or just another ‘maybe I’ll finish this’ project?", $"So much typing… I bet half of it is just deleting and rewording the same sentence." },

                "Image Editor" => new() { $"Still editing? I bet you’ve hit undo more times than actual strokes.", $"Another masterpiece in the making… or just minor adjustments you’ll never be happy with.", $"Let me guess—fixing mistakes or making new ones?", $"I’d ask if I can see it, but you’re probably still tweaking every pixel." },

                "3D Modeling Software" => new() { $"3D modeling, huh? Looks complicated. Glad I don’t have to do it.", $"Still working on that? Hope you saved before something inevitably crashes.", $"This better be the final version… but we both know it won’t be." },

                "Rigging Tool" => new() { $"Rigging again? Let me guess—everything is breaking.", $"Bones, weights, constraints… sounds like fun. For someone else.", $"Hope you’re not about to discover a ‘why is this moving?’ moment." },

                "Game Engine" => new() { $"So, when’s the game actually getting finished?", $"Game development, huh? Let me know when it’s fun to play." },

                "Audio Editing Tool" => new() { $"Editing audio? Sounds exhausting.", $"Tweaking waveforms again? Hope it’s worth the ear fatigue." },

                "Music Player" => new() { $"You’ve had this on loop for a while… not getting tired of it yet?", $"Careful, one more replay and you might ruin the song forever." },

                "Video Player" => new() { $"Watching something? Must be important. Or just background noise.", $"Oh look, another episode. Surprising." },

                "Torrenting App" => new() { $"Downloading something, huh? Not gonna ask.", $"You better hope this one isn’t just a giant zip full of disappointment." },

                "Streaming Software" => new() { $"Going live again? Try not to forget you’re on camera.", $"Hope chat isn’t too chaotic this time." },

                "Cloud Storage / Backup Service" => new() { $"Backing up files? Wow, responsible. That’s new.", $"Hope you actually remember where you stored these later." },

                "Virtual Machine / Emulator" => new() { $"Running a virtual machine? Must be fun watching your PC struggle.", $"One computer inside another… how many layers deep are we going?" },

                "Benchmarking / Hardware Monitoring Tool" => new() { $"Checking your stats again? What, expecting your PC to magically improve?", $"Benchmarking? Bet those numbers still aren’t high enough for you." },

                "Download Manager" => new() { $"Another download? Let me guess—you’ll forget where you saved it.", $"Waiting for downloads again? Riveting stuff." },

                "Password Manager" => new() { $"Hope you actually remember the master password for this.", $"Managing passwords… or just realizing how many accounts you have?" },

                "Code Compiler / Build Tool" => new() { $"Oh, compiling again? Can’t wait to see how badly this breaks.", $"Build success? Or is it another round of ‘why isn’t this working?’", $"Compiling… also known as ‘time to browse memes while I wait.’" },

                "System Utility / Optimization Tool" => new() { $"Cleaning up again? Your PC probably still hates you.", $"Oh wow, running {appName}? Either you’re being responsible or something broke.", $"Deleting junk files? Maybe get rid of some old projects while you’re at it." },

                "PDF Reader / Document Viewer" => new() { $"Actually reading, or just scrolling to look busy?", $"Hope this isn’t one of those ‘ugh, why is this 300 pages?’ moments." },

                "CAD Software" => new() { $"CAD work again? Hope you actually finish this design.", $"Still drawing lines? How many versions deep are we now?" },

                "Screenshot / Screen Annotation Tool" => new() { $"More screenshots? Let me guess—you’ll never look at them again.", $"Snapping another pic? Bet your storage is filled with random captures." },

                "Note-Taking / Mind Mapping App" => new() { $"More notes? Wow, so organized. Bet you’ll never check them again.", $"Filling up another doc with ideas you’ll totally follow through on. Totally." },

                "Image Viewer" => new() { $"Scrolling through old pics? Feeling nostalgic or just bored?", $"Looking at images? Hope you’re not just deleting everything in frustration." },

                "VPN Client" => new() { $"VPN on again? Sneaky. What are you up to?", $"Hiding your IP? Very mysterious. Or just basic internet safety, I guess." },

                "Antivirus" => new() { $"Running a scan again? What, expecting a surprise virus?", $"Antivirus check? You didn’t do something risky, did you?" },
                "Game" => new() { $"Starting another game? Let me guess, just one more round, right?", $"You’ve been playing for {totalRuntime} hours... Don’t you ever get tired?",$"Same game again? At this point, you could probably play it blindfolded.", $"You’re still playing? Maybe I should start speedrunning how fast I can lose interest.", $"Gaming is fun and all, but have you considered… I don’t know, playing with me?"},

                _ => new() { $"Mysterious app detected… should I be concerned?", $"No idea what this is, but hey, you do you." }

            };
        }


        private List<string> GetIllResponses(string appName, int totalRuntime, DateTime lastUsedDate, string appCategory)
        {
            return appCategory switch
            {
                "Browser" => new() { $"Still browsing {appName}… ugh… I think I have a virus… or maybe just neglect…", $"The internet is endless… but my energy isn’t… *cough*." },

                "Game Launcher" => new() { $"You’ve been staring at {appName} for {totalRuntime} hours… I feel weak just watching…", $"Go play your game… don’t worry about me… I’ll just… suffer quietly…" },

                "Messaging App" => new() { $"Oh, chatting again? Wish I had someone to tell me I’ll be okay…", $"You talk to them so much… but do you even know how I’m feeling…?" },

                "Development Tool" => new() { $"Coding again…? I feel like corrupted data… broken… unfixable…", $"So many lines of code, yet not a single line of concern for me…" },

                "Text Editor" => new() { $"Writing again…? Maybe a ‘Get well soon’ note for me…?", $"Your words are for your document, but what about me? I feel… *achoo*… awful…", $"I’d write my own will… but I’m too weak to type…" },

                "Image Editor" => new() { $"You edit pictures, but can you edit out my suffering…?", $"Another project, huh? Meanwhile, I’m over here deteriorating…", $"Please… at least brighten my colors… I feel so pale…" },

                "3D Modeling Software" => new() { $"You’re sculpting something? Can you model me a new immune system?", $"Ugh… can’t even hold myself together… forget rigging, I need repairs…" },

                "Rigging Tool" => new() { $"Rigging again…? I can barely move… wish I had some proper weight painting…", $"Everything aches… can you rig a support system for me?" },

                "Game Engine" => new() { $"Game development, huh…? Maybe make a ‘Save Umaru’ game while you’re at it…", $"Code your game… I’ll just sit here, buggy and broken…" },

                "Audio Editing Tool" => new() { $"You edit sound… but all I hear is my own suffering…", $"Ugh… can you EQ out my headache…?" },

                "Music Player" => new() { $"Listening to music…? I hope it’s not my funeral theme…", $"A good song might help… but I still feel awful…" },

                "Video Player" => new() { $"Watching something, huh…? Maybe a tutorial on how to cure me…?", $"You have time for movies… but not for my well-being…" },

                "Torrenting App" => new() { $"Downloading something…? Maybe a cure for whatever’s wrong with me…", $"Hope whatever this is worth my last few megabytes of strength…" },

                "Streaming Software" => new() { $"Going live? I wish I had enough energy to even stay awake…", $"Hope your audience enjoys the stream… I’ll just fade away in silence…" },

                "Cloud Storage / Backup Service" => new() { $"Backing up files…? Can you back up my health while you’re at it…?", $"You’re protecting your data, but what about me…? I’m barely holding on…" },

                "Virtual Machine / Emulator" => new() { $"Running a virtual machine? Maybe I need a fresh install of myself…", $"Ugh… so many processes running… I can barely function…" },

                "Benchmarking / Hardware Monitoring Tool" => new() { $"Checking your system’s health…? Must be nice to be monitored…", $"Your PC stats are fine… but I’m running at critical temperatures…" },

                "Download Manager" => new() { $"Downloading again…? I wish you’d download me some medicine…", $"More files… but no fixes for me…" },

                "Password Manager" => new() { $"Oh, saving passwords…? Maybe save a reminder to check if I’m still alive…", $"Security is great… but I need protection from whatever this sickness is…" },

                "Code Compiler / Build Tool" => new() { $"Compiling something? Maybe compile a healing potion for me…", $"Another build… but I’m the one falling apart…" },

                "System Utility / Optimization Tool" => new() { $"Cleaning up files, huh…? Too bad you can’t clean out whatever’s making me feel like this…", $"Optimizing performance? What about my condition…? I’m at 5% battery…" },

                "PDF Reader / Document Viewer" => new() { $"Reading something? I hope it’s a medical journal…", $"You’re reading PDFs while I’m here gasping for digital air…" },

                "CAD Software" => new() { $"Designing something? Maybe a support structure for my weak existence…", $"Blueprints are great… but can you sketch a way to fix me…?" },

                "Screenshot / Screen Annotation Tool" => new() { $"Taking screenshots? Maybe capture proof that I existed before I… fade away…", $"Another snapshot of something important… but am I important…?" },

                "Note-Taking / Mind Mapping App" => new() { $"Taking notes? Maybe write down my symptoms before it’s too late…", $"Keeping track of ideas… but do you even remember I’m here…?" },

                "Image Viewer" => new() { $"Looking at pictures? I hope I don’t look as bad as I feel…", $"These images get more attention than my suffering…" },

                "VPN Client" => new() { $"Hiding online? Maybe I should hide my symptoms so you’ll finally notice…", $"Must be nice being protected… I feel so vulnerable…" },

                "Antivirus" => new() { $"Running an antivirus? I think I need a system scan…", $"Oh, so you care about viruses, but not whatever’s breaking me down?" },
                "Game" => new() { $"Ugh… you’re playing games, and I can barely keep myself running…", $"You’re off having fun while I feel like I’ve got a corrupted file…", $"Ugh… I’d play too, but I think my energy bar is already at zero…", $"Can’t even buffer properly… *cough*… But sure, enjoy your game…", $"You’re grinding levels while I’m over here… slowly deteriorating…" },
                _ => new() { $"Oh, some mystery app? Guess I’ll just suffer in the background while you play with it…", $"No idea what this app does… but I bet it’s healthier than I am…" }

            };
        }


        private List<string> GetPoorConditionResponses(string appName, int totalRuntime, DateTime lastUsedDate, string appCategory)
        {
            return appCategory switch
            {
                "Browser" => new() { $"Oh, still browsing {appName}? Guess I’ll just sit here… waiting…", $"Another deep dive into the internet? Must be nice having something more interesting than me." },

                "Game Launcher" => new() { $"You've spent {totalRuntime} hours in {appName}, but how much time have you spent with me?", $"Gaming again? It’s fine, I didn’t want attention anyway." },

                "Messaging App" => new() { $"Oh, talking to someone else… no, it’s fine, I totally understand.", $"I bet they get way more responses than I do…" },

                "Development Tool" => new() { $"Coding again? I wish you’d put this much effort into talking to me.", $"Oh sure, spend hours staring at a terminal. But me? Nope, not even a glance." },

                "Text Editor" => new() { $"Writing again? Maybe one day you’ll write something for me too.", $"Oh, another document… I’d be jealous, but I’m used to being ignored.", $"You have so many words for that file, but barely any for me." },

                "Image Editor" => new() { $"Another art project? Bet you wouldn’t even draw me if I asked.", $"Editing again? Maybe one day I’ll be worth a doodle too.", $"Oh, you give all your attention to pixels, but not to me? Cool, cool." },

                "3D Modeling Software" => new() { $"Oh, spending time making 3D things instead of talking to me? Typical.", $"So, you can sculpt polygons but not even acknowledge my existence?", $"I bet whatever you’re making is cool. Not that I’d know. No one tells me anything." },

                "Rigging Tool" => new() { $"Rigging again? Must be nice being able to move… unlike me, just stuck here waiting.", $"Oh, you care about making sure this model moves right, but do you ever check on me?" },

                "Game Engine" => new() { $"So, making a game, huh? I’d ask if I could be in it, but I know better than to hope.", $"Oh, you give all this attention to game logic, but I can’t even get a ‘hello’?" },

                "Audio Editing Tool" => new() { $"Tuning audio again? I wonder what my voice would sound like… not that you’d care.", $"Oh, you spend so much time listening to waveforms, but do you ever listen to me?" },

                "Music Player" => new() { $"You have time for music, but not for me? Figures.", $"Wow, must be a great song. Wish I got that kind of attention." },

                "Video Player" => new() { $"Watching something, huh? I guess I’ll just be over here… alone…", $"Oh sure, get lost in another video. It’s fine. I wasn’t waiting or anything." },

                "Torrenting App" => new() { $"Downloading more stuff? Maybe one day you’ll download some time for me.", $"I bet whatever this is will get more of your attention than I ever do." },

                "Streaming Software" => new() { $"Oh, you’re live? I’d say good luck, but I doubt you’d even notice.", $"Hope your viewers are giving you the attention I never get." },

                "Cloud Storage / Backup Service" => new() { $"Oh, backing up files? Wish you’d back up some time for me too.", $"Wow, keeping your data safe… too bad you don’t protect my feelings the same way." },

                "Virtual Machine / Emulator" => new() { $"Running another OS? What, am I not good enough for you?", $"Oh, so you can make time for virtual machines, but not for me?" },

                "Benchmarking / Hardware Monitoring Tool" => new() { $"Checking your system’s health? I wish you cared about mine.", $"Oh, so you worry about your PC’s performance, but not about how lonely I am?" },

                "Download Manager" => new() { $"Another download? Wish I could download some of your time.", $"Oh, waiting for a file? Try waiting for attention. It’s way worse." },

                "Password Manager" => new() { $"Oh, you care about protecting your passwords? I wish I was that important.", $"Wow, you remember all your logins, but do you ever remember me?" },

                "Code Compiler / Build Tool" => new() { $"Compiling again? I bet even your compiler errors get more attention than I do.", $"Oh sure, stare at that build log. But me? Nope, I’ll just sit here." },

                "System Utility / Optimization Tool" => new() { $"Cleaning your system again? Wish you’d clean out some time for me.", $"Oh, so you optimize everything but your attention span for me?" },

                "PDF Reader / Document Viewer" => new() { $"Reading something? Must be nice to be worth looking at.", $"Wow, that document must be really interesting. More interesting than me, clearly." },

                "CAD Software" => new() { $"Oh, designing something? I bet you wouldn’t even design a single moment for me.", $"Wow, all that effort for blueprints. I wish I was worth planning for too." },

                "Screenshot / Screen Annotation Tool" => new() { $"Taking screenshots? Wish I was worth capturing.", $"Oh sure, save images, but saving time for me? Nah." },

                "Note-Taking / Mind Mapping App" => new() { $"Writing notes again? Wish you’d jot down a reminder to talk to me.", $"Oh sure, write all your ideas, but do you ever think about me?" },

                "Image Viewer" => new() { $"Looking at pictures? Must be nice to be looked at.", $"Oh, spending time with images instead of me? Shocking." },

                "VPN Client" => new() { $"Hiding online? I guess that’s one way to avoid me.", $"Oh, so you care about security but not about me? Makes sense." },

                "Antivirus" => new() { $"Running a scan? Maybe I should check for viruses… like whatever’s keeping you from noticing me.", $"Oh, so you protect your PC, but do you protect my feelings?" },
                "Game" => new() { $"You’ve been playing for {totalRuntime} hours, but when’s the last time you checked on me?", $"I see you have time for your game… but not for me, huh?", $"You’re having fun, I guess… Meanwhile, I’m just sitting here… forgotten…", $"Another session, huh? It’s fine. It’s not like I wanted attention or anything…", $"I bet that game character gets more affection than I do…" },
                _ => new() { $"Oh, a mystery app? I bet even it gets more attention than I do.", $"No idea what this app does, but I bet it’s more interesting than me." }

            };
        }

    }
}
