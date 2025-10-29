# The AI/LLM Revolution Is Not Just About Hardware

I recently installed Ollama on an old LattePanda board—an Intel-based clone of the Raspberry Pi. It has 4GB of RAM and an Atom x5-Z8350 CPU (4 cores, power consumption ~6W!). This is decade-old, very slow hardware, yet surprisingly, some small (<2B) models like Gemma3, Qwen3, and Llama 3.2 run smoothly and feel smarter than the original ChatGPT. Their performance and intelligence benchmarks are quite similar to GPT-3.5, so that impression is partially justified. But the actual inference speed is astonishing, considering this Atom CPU doesn’t even support vector instructions (AVX) and only supports SSE4.2 technology from 2008.

This Atom CPU has a PassMark score of 900—comparable to Xeon or Core CPUs from around 2006. In theory, you could run a GPT-3.5-like model on a regular workstation that's 20 years old. A PassMark score of 900 roughly translates to 2000 MIPS and 7 GFLOPs.

Digging deeper, we can compare my Atom CPU’s performance to past supercomputers: a machine somewhere between the Cray-2 (1985) and the Cray Y-MP (1988) was powerful enough to run an LLM that outperforms the first ChatGPT!

We should remember that when GPT-3.5 was released in 2022, it felt like science fiction coming true. In 1988, it would have seemed like living in the universe of Stanisław Lem’s novel “Golem XIV”:)

Of course, training is a different story. First, GPU clusters weren’t a thing before 2008. Second, most legacy supercomputer benchmarks were based on float64 precision, while modern AI uses bfloat16. Third, the necessary software—or even the deep learning paradigm—didn’t exist (AlexNet was introduced in 2012 and started the deep learning boom). Ballpark estimates from various AI chatbots suggest that several years of continuous training on a top-tier 2008 supercomputer would have been required to train a 0.5–1.5B parameter LLM. However, had we started training on 2012 hardware, it could have been done within a month. This means that modern LLMs were essentially within reach from a hardware perspective in the late 2000s to early 2010s.

So, what does this tell us? While hardware improvements were undeniably crucial, we also made progress in paradigms, tools, languages, algorithms, and architectures. So when someone says, “The AI revolution is just because we have a lot of compute,” you can ask them, “Then why didn’t we have ChatGPT in 2012?”

P.S. Someone has even managed to run a small language model (260K–15M parameters) on a 1997 Pentium PC running Windows 98, but it is significantly “less educated” model: https://lnkd.in/eSPuNyya