# AI Local

TL;DR If you install Visual Studio Code + Continue (plugin) and Ollama + llama3.1 model, you can have an offline coding assistant on your laptop (as long as it's new and fancy). This makes it safe for medical, defense, and finance data.

## How?

AI coding assistance might not do all the work for you, but it's a great tool. 
Microsoft's GitHub Copilot, OpenAI's Codex, Google's Gemini Code Assistant, and Anthropic's Claude Code can all make your Visual Studio Code experience great :). But there's one problem: what if you're not allowed to use these tools?

There are a couple of reasons why you might not want to use public/open tools. Even if we assume that there are no hostile intentions from the parent companies, and that your conversation won't be used as training data (and this last assumption is a stretch), it's still possible that there could be a data leak of historical conversations with LLMs, or that someone might intercept the communication.

So if you're working in a defense company or hedge fund, the risk of revealing secrets in your code (and the agent needs to read all of it to be helpful!) might be too risky. And if you're working with medical records, even sharing error messages in ChatGPT to get some hints on what's wrong with your data analysis script is risky, because you'd have to share patient data. For similar reasons, companies tend to limit access to tools used for proofreading emails, as you don't want a trade secret about a new release to leak, even accidentally.

But this doesn't mean that in such scenarios you're cut off from modern coding assistants or chatbots. You also don't have to copy & paste your code snippets to ChatGPT to get help, or - which is worse - take a photo of your computer screen with your smartphone and then send the entire picture to ChatGPT for help.

You can run LLM chatbots on your private PC, fully locally, air-gapped - with no internet access, etc. And it doesn't have to be super powerful machines; modern laptops are usually enough.

## Chatbots

For chatbot-like functionality, I recommend two alternative tools:
a) [GPT4All](https://www.nomic.ai/gpt4all)
![GPT4ALL](localAI/gpt4all_chat.jpg)
b) [Ollama](https://ollama.com/)
![Ollama](localAI/ollama_ui.jpg)
The first one has a better UI; the second one can also be used as a coding agent local backend. These tools support multiple models and GPU (if available) out-of-the-box.

Models that I recommend trying are:
* Google's Gemma3 - it's multimodal, so you can use both image + text as input
* Deepseek - currently probably the best one to run locally
* Llama3 - from Meta, can be used as a general-purpose model
* Mistral - a European model, with a lesser degree of censorship

If you're not familiar with LLM models, the "8b" or "12b" means model size. Rule of thumb: you need at least 0.5GB (RAM) or 0.8GB (GPU RAM) of memory to run it.

GPT4All's interface as a chatbot has a very straightforward interface; pretty much you just have to download it, install it, and pick a model, and it will download it automatically.
![GPT4ALL install](localAI/gpt4all_install.jpg)

## Local coding assistance

But working with chatbots is kind of manual, inefficent way of doing things. New coding assistance tools are much more convenient if you're working with code. So instead of copying & pasting between your IDE and chatbot, you can let it read and modify your code directly.

The setup which I tried relies on [Visual Studio Code](https://code.visualstudio.com/). If you've never used it - try it, it's a very good tool. It has support/extensions for multiple languages, including obviously Python, JavaScript, C++, Rust, and some less popular languages used by scientists like R, MATLAB, and Julia.

On top of Visual Studio Code, you have to install an extension called Continue, which is an open-source coding assistant. It supports both external/public backends like ChatGPT, but if you want to use it like that, there are more straightforward tools like GitHub Copilot. The true strength is that [Continue.Dev](https://marketplace.visualstudio.com/items?itemName=Continue.continue) can use a **local** backend. So none of your data will be sent anywhere, and you can even disable internet on your machine or work from a nuclear submarine.

Continue.Dev supports Ollama as a backend. To use it as an agent, and not just as a chatbot, you have to pick a model that supports the *tools* protocol.

I picked Llama3.1:8B as I'm running it on quite old hardware.

After you've installed Ollama and the model, just make sure that it runs using:
```bash
ollama serve
```
It should show something like that:
``` 
Error: listen tcp 127.0.0.1:11434: bind: address already in use
```

Which means that Ollama is already running. 

Then you have to modify the configuration of Continue.Dev and voila, you can tell you agent to do the Hello World for you:

As you can see I have following models installed
```bash
$ ollama pull llama3.1:8B
$ ollama pull qwen2.5-coder:1.5b-base
$ ollama pull nomic-embed-text:latest
$ ollama pull deepseek-r1:1.5b
```

Afterwards you have to set up the VSCode Continue.Dev extension:
![continue's adding model ](localAI/vsc_model_setup1.jpg)
![continue's adding model ](localAI/vsc_model_setup2.jpg)
![continue's adding model ](localAI/vsc_model_setup3.jpg)
Then you can set modify `.continue/config.yaml` file 
![continue's setup](localAI/vsc_continue_config2.jpg)
My configuration is:
```yaml
name: Local Agent
version: 1.0.0
schema: v1
models:
  - name: Llama 3.1 8B
    provider: ollama
    model: llama3.1:8b
    roles:
      - chat
      - edit
      - apply
  - name: DeepSeek R1 1.5B
    provider: ollama
    model: deepseek-r1:1.5b
    roles:
      - chat
      - edit
      - apply      
  - name: Qwen2.5-Coder 1.5B
    provider: ollama
    model: qwen2.5-coder:1.5b-base
    roles:
      - autocomplete
  - name: Nomic Embed
    provider: ollama
    model: nomic-embed-text:latest
    roles:
      - embed
  - name: Llama 3.1 8B
    provider: ollama
    model: llama3.1:8B
    roles:
      - chat
      - autocomplete
      - edit
      - rerank
    capabilities:
      - tool_use
```



The Continue.Dev extension has multiple ways of helping the developer:
a) local AI-based code completion
b) Ask: chatbot mode
c) Agent/Edit: creating files, running commands
[!Continue.Dev UI](localAI/vsc_continue_open.jpg)


## Hello World aka real-life use cases
### Factorial in C++
![factorial](localAI/vsc_continue_cpp.png)
You can notice, that the generated code is correct, however a comment is a little bit wrong - code uses `long long int` not `int`.
![factorial](localAI/vsc_continue_cpp3.png)
Agent can even compile and run the code automatically:
![factorial](localAI/vsc_continue_run.png)

### Help with R
I don't use R, I don't know it & I don't like it:)
But I know that it is widely used by biotech research community.

VSCode + Continue.Dev + R Extension does the work:
![vs r continue](localAI/vsc_continue_r.png)
And it is fully local, not sending data anythere. You can be in Antarctica, disconnected from the grid and doing some climate research and still use modern AI:)

## The End / What else
Unfortunately, my setup was quite slow in agent mode. Chatbots on your local PC are much faster. But if you're working on a secret project for S.P.E.C.T.R.E., they can afford to buy you a new fast laptop (like the latest MacBook with M3 CPU) or a desktop with a nice NVIDIA GPU, or even set up a local GPU cluster that will be used just as a coding tool backend.

Obviously, when you're using these tools, you have to remember that local smaller models can make even more mistakes than the huge public ones. But sometimes these tools are very useful.

Links that are worth trying:
* [Tutorial for Continue.Dev](https://docs.continue.dev/guides/ollama-guide) 
* [R extension for VSCode](https://marketplace.visualstudio.com/items?itemName=REditorSupport.r)
* [Erdos](https://www.lotas.ai/erdos) - R-dedicated VSCode clone with non-local AI coding agent support. 
* [OnPerm LLM](https://amaiya.github.io/onprem/) - tools for powerusers to set up entire RAG locally

