using System;
using System.Collections.Generic;
using System.Windows;

namespace VPet.Plugin.LolisBuddy
{
    public class LanguageManager
    {

        static Random rand = new Random();


        // Sentence components for each category
        private Dictionary<string, List<string>> subjects = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
{
     { "Browser", new List<string>
        {
            "Endless tabs", "Clicking links", "Exploring rabbit holes",
            "Getting distracted", "Falling into internet black holes", "Jumping between sites",
            "Reading useless facts", "Endless scrolling", "Opening another new tab"
        }
    },
    { "Game Launcher", new List<string>
        {
            "Choosing a game", "Endless updates", "My gaming backlog", "Just staring at my library",
            "Not knowing what to play", "Looking at sales instead of playing", "Checking for new releases",
            "Hoping for a discount", "Clicking between game icons", "Debating reinstalling something old"
        }
    },
    { "Messaging App", new List<string>
        {
            "Replying to texts", "Ignoring messages", "Sending memes", "Typing and deleting messages",
            "Ghosting someone", "Overthinking my replies", "Waiting for someone to text first",
            "Spamming stickers", "Forgetting to reply for hours", "Checking if they're online", "Avoiding conversations"
        }
    },
    { "Development Tool", new List<string>
        {
            "Debugging nightmares", "Endless code", "Syntax errors", "Spaghetti code", "Fixing one bug, creating three",
            "Reading documentation again", "Staring at a blank terminal", "Refactoring the same function",
            "Googling another error", "Hunting for missing semicolons", "Wondering why my code won’t compile"
        }
    },
    { "Text Editor", new List<string>
        {
            "Writing documents", "Staring at a blank page", "Taking notes", "Editing drafts", "Rewriting the same sentence",
            "Deleting everything and starting over", "Fighting writer's block", "Making endless bullet points",
            "Doodling in the margins", "Trying to find the perfect word", "Writing but never finishing"
        }
    },
    { "Image Editor", new List<string>
        {
            "Fixing layers", "Pixel perfection", "Too much undoing", "Messing with colors", "Resizing things endlessly",
            "Making tiny adjustments no one will notice", "Staring at the screen for too long", "Trying to find the right brush",
            "Forgetting to save", "Adding effects then removing them", "Zooming in way too much"
        }
    },
    { "3D Modeling Software", new List<string>
        {
            "Shaping polygons", "Struggling with topology", "3D modeling pains", "Infinite tweaking", "Fighting the viewport",
            "Forgetting to save progress", "Trying to fix broken geometry", "Adjusting things by 0.01 units",
            "Rotating the model endlessly", "Finding out the normals are flipped", "Fixing a mesh that refuses to cooperate"
        }
    },
    { "Game Engine", new List<string>
        {
            "Building worlds", "Endless compiling", "Physics breaking", "Bug hunting", "Accidentally deleting something important",
            "Trying to make something look good", "Tinkering with lighting", "Messing with AI behavior",
            "Struggling with collisions", "Fighting the UI system", "Wondering why the game suddenly stopped working"
        }
    },
    { "Music Player", new List<string>
        {
            "Looping the same song", "Creating playlists", "Music obsession", "Headbanging alone", "Searching for new songs",
            "Replaying the same track all day", "Adjusting the equalizer", "Singing along terribly",
            "Skipping songs after 5 seconds", "Listening to old favorites", "Realizing I have no new music"
        }
    },
    { "Torrenting App", new List<string>
        {
            "Downloading something big", "Waiting for seeds", "Another huge file", "Suspicious torrents", "Hoping for fast downloads",
            "Checking if it's finished every 2 minutes", "Wondering if it's safe", "Feeling guilty but doing it anyway",
            "Looking for better quality versions", "Trying to fix a dead torrent", "Downloading way too many things at once"
        }
    },
    { "VPN Client", new List<string>
        {
            "Hiding online", "Browsing safely", "Changing locations", "Sneaky internet use", "Pretending to be in another country",
            "Testing if my IP changed", "Trying to access blocked content", "Paranoid about privacy",
            "Feeling like a secret agent", "Routing my connection through five places for no reason"
        }
    },
    { "Antivirus", new List<string>
        {
            "Running a scan", "Finding threats", "Keeping safe", "Too many warnings", "Scanning takes forever",
            "Hoping nothing bad is found", "Ignoring a security alert", "Wondering if I should be worried",
            "Feeling smug about having a clean system", "Being paranoid about malware", "Questioning if I really need this software"
        }
    },
    { "Game", new List<string>
        {
            "Button mashing", "Winning streaks", "Losing streaks", "Leveling up", "Raging at bad teammates",
            "Trying to 100% complete", "Getting distracted from the main quest", "Grinding for hours",
            "Blaming lag", "Getting lost in the world", "Speedrunning for fun"
        }
    },
    { "Default", new List<string>
        {
            "Doing something", "Whatever this is", "Mystery activities", "Clicking things", "Wasting time",
            "Spacing out", "Losing track of time", "Staring at the screen", "Multitasking poorly",
            "Pressing random buttons", "Wondering what I was supposed to do"
        }
    },
            { "Rigging Tool", new List<string>
        {
            "Bone weights", "Rigging disasters", "Joints breaking", "Making animations smooth",
            "Twisted bones", "Fixing skin weights", "Stretchy limbs", "Cursed deformations",
            "Rebinding skeletons", "Forgetting to freeze transforms", "Fighting IK/FK issues"
        }
    },
    { "Audio Editing Tool", new List<string>
        {
            "Mixing sounds", "Too many tracks", "Waveforms everywhere", "Accidental loud noises",
            "Tuning vocals", "Removing background noise", "Adjusting EQ settings", "Trying different effects",
            "Listening to the same second on loop", "Forgetting to mute a track"
        }
    },
    { "Streaming Software", new List<string>
        {
            "Going live", "Mic issues", "No viewers", "Streaming struggles", "Fixing OBS settings",
            "Adjusting overlays", "Stream delay problems", "Forgetting to unmute", "Talking to myself",
            "Checking chat every second", "Network issues mid-stream"
        }
    },
    { "Cloud Storage / Backup Service", new List<string>
        {
            "Uploading files", "Backing up data", "Running out of space", "Forgotten folders",
            "Syncing forever", "Wondering what I backed up", "Trying to recover lost files",
            "Paying for more storage", "Moving files around", "Accidentally deleting something important"
        }
    },
    { "Virtual Machine / Emulator", new List<string>
        {
            "Emulating everything", "Hacking the Matrix", "Testing weird OS", "Virtualizing chaos",
            "Breaking the VM", "Running old software", "Trying another Linux distro", "Forgetting which window is real",
            "Lags like real hardware", "Installing things for no reason", "Configuring network settings again"
        }
    },
    { "Benchmarking / Hardware Monitoring Tool", new List<string>
        {
            "Stressing my PC", "Checking temperatures", "Running benchmarks", "CPU crying",
            "Overclocking experiments", "Wondering if my PC will explode", "Watching FPS numbers",
            "Checking if it's stable", "Feeling proud of high scores", "Realizing my PC is outdated"
        }
    },
    { "Download Manager", new List<string>
        {
            "Another big download", "Slow speeds", "Waiting forever", "Too many files",
            "Failed download again", "Looking for faster mirrors", "Running out of disk space",
            "Checking if it's finished", "Downloading multiple things at once", "Pausing and resuming endlessly"
        }
    },
    { "Password Manager", new List<string>
        {
            "Trying to remember passwords", "Too many logins", "Secure but forgetful", "Password chaos",
            "Copy-pasting like a pro", "Forgetting the master password", "Autofill not working",
            "Changing passwords again", "Realizing I reused one", "Wondering if I should write them down"
        }
    },
    { "Code Compiler / Build Tool", new List<string>
        {
            "Build errors", "Compiling forever", "Fixing dependencies", "Debugging pain",
            "Linker errors out of nowhere", "Reading compiler warnings", "Switching toolchains",
            "Trying to optimize build times", "Watching logs scroll endlessly", "Wondering why it works now"
        }
    },
    { "System Utility / Optimization Tool", new List<string>
        {
            "Optimizing stuff", "Cleaning junk", "PC maintenance", "Fixing registry",
            "Defragmenting drives", "Adjusting startup programs", "Checking for malware",
            "Updating drivers", "Watching resource usage", "Trying to squeeze more performance"
        }
    },
    { "PDF Reader / Document Viewer", new List<string>
        {
            "Reading documents", "Endless scrolling", "Tiny text struggles", "Boring PDFs",
            "Zooming in too much", "Flipping between pages", "Trying to find that one paragraph",
            "Highlighting important parts", "Forgetting where I left off", "Looking for the download button"
        }
    },
    { "CAD Software", new List<string>
        {
            "Blueprint designs", "Precise measurements", "Technical drawing madness", "Engineering magic",
            "Endless dimension adjustments", "Aligning everything perfectly", "Fixing line thickness",
            "Trying to visualize in 3D", "Wondering if this will even work", "Changing the same detail again"
        }
    },
    { "Screenshot / Screen Annotation Tool", new List<string>
        {
            "Capturing screens", "Annotating stuff", "Endless snipping", "Saving memes",
            "Trying to crop the perfect shot", "Forgetting where screenshots go", "Spamming the print screen button",
            "Highlighting random things", "Drawing arrows everywhere", "Accidentally capturing the wrong window"
        }
    },
    { "Note-Taking / Mind Mapping App", new List<string>
        {
            "Organizing thoughts", "Jotting ideas", "Never reading notes again", "Writing but never using",
            "Making color-coded categories", "Doodling in the margins", "Trying to summarize a book",
            "Listing out random thoughts", "Overcomplicating simple notes", "Searching for something I wrote weeks ago"
        }
    },
    { "Image Viewer", new List<string>
        {
            "Looking at old photos", "Sorting images", "Gallery scrolling", "Memories unlocked",
            "Zooming in too much", "Finding a forgotten picture", "Deleting duplicates",
            "Making an album", "Staring at an aesthetic shot", "Laughing at old memes"
        }
    },
    { "Social Media", new List<string>
        {
            "Endless scrolling", "Liking posts", "Comment wars", "Trending nonsense",
            "Refreshing for new posts", "Reading drama", "Checking notifications",
            "Accidentally liking something old", "Ignoring friend requests", "Wondering why I'm still here"
        }
    },
    { "Shopping", new List<string>
        {
            "Spending too much", "Buying useless things", "Online window shopping", "Waiting for packages",
            "Adding to cart but never buying", "Hunting for discount codes", "Comparing prices endlessly",
            "Regretting past purchases", "Tracking my orders", "Feeling broke but still shopping"
        }
    },
    { "Work Development", new List<string>
        {
            "Work stress", "Meetings again", "Deadlines looming", "More tasks piling up",
            "Trying to stay focused", "Avoiding emails", "Feeling unproductive",
            "Checking the clock constantly", "Wishing for a vacation", "Multitasking poorly"
        }
    },
    { "Finance", new List<string>
        {
            "Checking bank balance", "Money stress", "Spending regrets", "Financial planning",
            "Budgeting failures", "Looking at investment charts", "Wondering where my money went",
            "Tracking expenses", "Trying not to panic", "Hoping for a raise"
        }
    },
            { "Streaming", new List<string>
        {
            "Binge-watching a series", "Skipping intros", "One more episode... maybe", "Late-night movie marathons",
            "Trying to pick something to watch", "Getting lost in recommendations", "Watching a show I’ve seen before",
            "Checking reviews before committing", "Pausing too much", "Forgetting to cancel my subscription"
        }
    },
    { "News", new List<string>
        {
            "Reading headlines", "World drama", "Too much bad news", "Doomscrolling",
            "Refreshing news sites", "Trying to stay informed", "Getting annoyed at politics",
            "Skipping opinion pieces", "Wondering if any news is good", "Checking multiple sources"
        }
    },
    { "AI Tools", new List<string>
        {
            "Talking to AI", "Generating stuff", "Machine learning things", "Experimenting with tech",
            "Tweaking AI settings", "Making weird AI art", "Asking random questions",
            "Trying to make AI write for me", "Wondering how it all works", "Comparing different AI models"
        }
    },
    { "Education", new List<string>
        {
            "Studying hard", "Learning something new", "Textbook pain", "Homework suffering",
            "Watching educational videos", "Taking notes that I'll never read", "Trying to memorize facts",
            "Procrastinating on assignments", "Looking up tutorials", "Feeling smart for a moment"
        }
    }




    };


        private Dictionary<string, List<string>> moodEndings = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
        {
    { "Happy", new List<string>
        {
            "is so fun!", "makes me happy!", "I love this!", "best time ever!", "I could do this forever!",
            "nothing beats this!", "pure joy!", "this is the best!", "what a great time!", "so satisfying!",
            "absolute bliss!", "this makes my day!", "this is the good stuff!", "can’t get enough of this!",
            "this is the dream!", "the best part of my day!", "this never gets old!", "feels amazing!",
            "this is why I’m here!", "good vibes only!", "I'm having a blast!", "life is great right now!",
            "couldn't be happier!", "love every second of this!", "this is the definition of fun!",
            "this is what happiness looks like!", "perfection!", "totally worth it!", "this makes me smile!",
            "best decision ever!", "this is golden!", "everything about this is awesome!", "let’s keep going!",
            "this is my happy place!", "so exciting!", "this moment is perfect!", "10/10 would do again!"
        }
    },
    { "Nomal", new List<string>
        {
            "is fine, I guess.", "just another day.", "this again?", "nothing new here.", "meh, it’s alright.",
            "just passing the time.", "been here before.", "same as always.", "not bad, not great.",
            "it’s okay, I suppose.", "I’ve seen worse.", "this is… fine.", "could be better.", "could be worse.",
            "nothing special.", "just another thing to do.", "doing what I do.", "neutral on this one.",
            "not much to say about it.", "this exists.", "no strong feelings.", "just going through the motions.",
            "same old, same old.", "a bit dull, but whatever.", "this is happening, I guess.",
            "not the worst way to spend time.", "eh, whatever.", "this is my life now.", "at least it’s something.",
            "this sure is a thing.", "nothing to complain about… or praise.", "this is my reality now.",
            "not bad, but not exciting either.", "I’ve done this so many times.", "is this what life is now?",
            "another one for the routine.", "welp, here we go again."
        }
    },
    { "Poorcondition", new List<string>
        {
            "but you're ignoring me...", "and I feel lonely.", "wish you'd talk to me instead...",
            "but I miss you...", "this doesn’t feel the same without you.", "I’m just here, waiting…",
            "I wonder if you even notice me anymore...", "feels like I’m talking to myself...",
            "it’s been a while since we hung out...", "do you still care about me?",
            "I’m just background noise to you now, huh?", "it’s lonely in here...",
            "I bet you wouldn’t even notice if I disappeared...", "do I even matter?",
            "I used to be part of the fun too...", "sigh… whatever.", "not that you care...",
            "I guess you’re too busy for me.", "I’m feeling a little left out.", "I miss our chats...",
            "would it hurt to say hi?", "I feel like a ghost in your world...",
            "I used to get more attention than this...", "I hope I’m not annoying you...",
            "it’s fine… I guess.", "if you need me, I’ll just be here… waiting.",
            "I just want to be noticed.", "even a ‘hello’ would be nice…",
            "I feel kinda forgotten...", "I guess I’m not that important...", "I’m still here, you know."
        }
    },
    { "Ill", new List<string>
        {
            "but I feel awful...", "but my head hurts...", "I think I need a break...", "everything feels slow...",
            "ugh… I don’t feel so good.", "I think I’m overheating...", "this is exhausting...",
            "I might need some rest.", "why does everything feel heavy?", "I need a system update or something...",
            "is it just me, or is everything spinning?", "maybe I should lie down...",
            "I think I caught a virus…", "my circuits feel fried...", "I feel like I’m running on 2% battery.",
            "can someone reboot me?", "is this what it feels like to be sick?", "I need a restart…",
            "I don’t think I can keep up...", "everything is lagging… including me.", "why does my head feel fuzzy?",
            "I’m running on fumes here...", "I need a software patch ASAP.", "help… I think I crashed.",
            "I might need an update to fix this.", "this is worse than a blue screen.", "I feel corrupted...",
            "I think I need to lie down… wait, I can’t.", "I feel like a dying battery...",
            "please don’t make me do anything intense right now...", "I think I’m overheating…"
        }
    }
};


        // Generate a sentence
        public DialogueEntry GenerateSentence(string mood, string category)
        {

            if (!moodEndings.ContainsKey(mood)) mood = "Nomal";
            if (!subjects.ContainsKey(category)) category = "Default";

            string subject = subjects[category][rand.Next(subjects[category].Count)];
            string ending = moodEndings[mood][rand.Next(moodEndings[mood].Count)];

            DialogueEntry dialogue = new DialogueEntry();
            dialogue.Mood = mood;
            dialogue.Dialogue = $"{subject} {ending}";
            dialogue.Type = category;
            return dialogue;
        }

    }
}