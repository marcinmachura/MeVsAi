# Programowanie / niezła sztuka
[English translation](0-EN-software-eng.md) (by AI)

Donald Knuth napisał kiedyś bardzo wartościową i skondensowaną książke "Sztuka programowania", tak że ta analogia jest już zajęta. Dlatego zacznę od tego czym programowanie nie jest: programowanie nie polega na tym by w jak najbardziej optymalny sposób instrukcjami maszynowymi wycisnąć ostatnie poty z jednostki centralnej (CPU) komputera. Owszem, w bardzo specyficznych zadaniach ta wiedza się przydaje, ale w praktyce cały kod jest tworzony przez kompilator, ktory dużo lepiej operuje na kodzie maszynowym i wszystkich detalach konkretnej architektury. Jak ktoś ostatni raz programował 16bitowy assembler na i8086 to się może mocno zdziwić co dziś procesory potrafią.

Programowanie w wielkim skrócie polega na przekładaniu nie do końca sprecyzowanych ludzkich pomysłów i zachcianek na język formalny, najlepiej tak by zachować możliwe niewielki stopień złożonośc/abstrakcji. Tzn. dużo trudniej jest napisać program w Pythone (niby najprostszym i niewydajnym języku) z rozmów z klientem, niż przepisać ten sam program w Pythonie do assemblera. Formalizm bowiem wymusza ubranie w kontkret / werbalizację wszystkich tych rzeczy i spraw o których nie myślimy definiując problem do rozwiązania. Owszem istnieją rozwiązania poprawne lepsze i gorsze: można przekombinować ze stopniem złożoności, dodać za dużo zależności będącymi potencjalnymi źródłami problemów, albo można po prostu zrobić coś bardzo niewydajnie. A ma "po prostu działać"


## O czym będzie ten artykuł?

O roli jaką języki programowania odegrały w kształtowaniu tego czym dziś jest świat IT. Bo z językami programowania jest podobnie jak z innym (nie)formalnymi standartami, jak dominacja QWERTY, portale społecznościwe czy VHS vs Betamax. Nie zawsze lepszy pomysł wygrywał, wiele zależało od tego by być w odpowiednim miejscu o odpowiednim czasie. 

Dziś możemy powiedzieć, że głównym beneficjentem tych szczęśliwych zbiegów okoliczności był Microsoft. Zaczął jako producent języka programowania i dbając o (względny) komfort programistów trzymał się w peletonie przez ostatnie pół wieku, czego już nie możemy powiedzieć np. o IBM.

Wbrew teoriom spiskowym nie dlatego, że Bill Gates był dobrze zakorzeniony w amerykańskiej klasie wyższej i służył władzy – wszak IBM, który właśnie przegrał bezpośrednią konfrontacje z Microsoftem był i pozostał dużo bardziej tworem Systemu – Big Blue to jest ucieleśnienie Corporate America. [por. Halt & Catch Fire; History of Microsoft @ Channel 9]

Inna plotka głosi, że to dlatego, że Microsoft zmonopolizował rynek oprogramowania. Ale to jest odwrócenie porządku: ponieważ osiągnął taki sukces udało mu się zmonopolizować rynek.
Na czym polegał ten sukces? Na odwróceniu paradygmatu biznesowego IT: dla Microsoftu hardware był tylko dodatkiem do software. Z oczywistych względów potencjalne zwroty z oprogramowania, którego powielanie jest darmowe, są dużo większe niż ze sprzętu, którego każdy egzemplarz trzeba wyprodukować by móc go sprzedać. Oczywiście po drodze są problemy: piractwo indywidualne i korporacyjne. Pierwsze jest wtedy gdy po prostu nie płacimy za oprogramowanie, a drugie gdy inna firma kopiuje 1:1 nasz produkt i robi to taniej, bo nie musi wymyślać co się sprzeda. Ale i tak efekt skali w biznesie oprogramowania jest tego warty i został przebity dopiero przez efekt skali w biznesie internetowym.

Sytuacja początku lat ‘80, gdy działa się wspomniana historia, nie była prostą Microsoft vs IBM, albo Microsoft vs Apple [Pirates of the Silicon Valley]. Przede wszystkim, żeby było komu sprzedawać oprogramowanie, ktoś musiał najpierw kupić komputer by mieć gdzie go uruchomić. Co prawda w szczycie potęgi Microsoftu, gdy Windows 95 wchodził do sklepów, dzięki kampanii reklamowej najnowszy system operacyjny kupowali także ludzie którzy NIE mieli komputera, no ale takich doskonałych klientów była garstka.

## Krótka systematyka maszyn liczących

Komputery mają wielu praojców: Euklides (algorytm NWD), mechanizm z Antykithry, lalki Pierre Jaquet-Droza, mechaniczne analizatory różnicowe (np. do przewidywania przypływów i odpływów), maszyna analityczna Charlesa Babbage’a (1833-1871), która nigdy nie powstała, ale wystarczyła by zbałamucić Adę Lovelace (córkę Lorda Byrona) do zostania pierwszą programistką. Potem była maszyna Turninga (też teoretyczna) i rachunek lambda Churcha. Na papierze wszystko działało, co dziś nie jest aż takie przekonujące, ale właśnie maszynę Turinga, jako uniwersalną przyjmujemy za najmniejszy standard komputera. Wkracza tu bardzo ładna matematyka o funkcjach pierwotnie rekurencyjnych, automatach skończonych albo ze stosem i obliczalności. W skrócie komputer jest wtedy gdy ma IF i GOTO, wtedy też język programowania jest turing complete.

Pierwsze praktyczne maszyny to dopiero przedświt II WŚ: Po pierwsze Z3 Konrada Zuse, budowana w Niemczech w 1935-41, Collosus Mark 1 budowany w Bletchley Park do łamania szyfrów (oparty o naszą Bombę) oraz maszyna Atanasoffa-Berriego (1939). Pierwszy był elektromechaniczny, drugi elektroniczny, trzeci w pełni elektroniczny (lampy) – ale jeszcze nie były turing complete. Dopiero amerykański ENIAC (1943-1945, używany do 1955) był programowalnym komputerem, choć programowanie sprowadzało się do zmiany okablowania.

Potem poszło z górki: w 1954 pojawia się produkowany seryjnie IBM 604 oparty już o tranzystory (1947), w 1958 powstają pierwsze układy scalone, a w 1964 IBM System 360 – pierwszy komputer z nich zbudowany. Komputery stawały się coraz mniejsze, coraz szybsze i coraz tańsze i wbrew przekręconemu cytatowi z 1953 szefa IBM Thomasa Watsona „na świecie potrzeba góra kilka komputerów” (w oryginale mówił o tym ile IBM dostał zamówień) rynek rósł.

Kolejny ważny kamień milowy to DEC PDP-8 (1964) pierwszy minikomputer – czyli dwudrzwiowa szafa zamiast zestawu meblościanek (mainframe jak IBM s/360). DEC i systemy PDP są o tyle warte zapamiętania, że to na PDP-11 w Bell Labs w Massachusetts Ken Thompson i Dennis Ritchie tworzą najpierw Unix (1969) a potem C (1972) i przepisują Unixa w C (1972). Początek naszej ery:) [kto nie rozumie dowcipu niech dalej nie czyta]

### Komputery domowe (aka ośmiobitowce)

Pierwszy ważny przełom którego czytelnik mógł doświadczyć na własnej skórze to mikroprocesor (1971, Intel 4004), mikrokomputer, oraz a 8-bitowe komputery domowe oparte o MOS 6500 (klon Motorolii 6500) i Zilog 80 – niby każdy inny, ale wszystkie do siebie podobne:

* Altair 8800 (1975), trochę zacofany bo z ograniczoną interaktywnością. To tu Bill Gates sprzedał pierwszy swój BASIC.
* Apple I (1976)
* Apple II (1977) – pierwszy prawdziwy minikomputer domowy
* Atari 400 (1979) – pierwszy mikrokomputer od masowego producenta – Atari rozwinęło się sprzedając w latach ’70 konsole do gier
* Commodore C64 (1982) (następca Commodore PETz 1977) – najpopularniejszy komputer wszechczasów.
* ZX Spectrum (1982) – Brytyjczycy zrobili go mocno „po taniości”
* Amrstard CPC 464 (1984), popularny w Niemczech, w zasadzie koniec 8bitowców

To był ten moment gdy „komputery trafiły pod strzechy” – pojawiła się scena, czasopisma, przegrywanie kaset z programami, przepisywanie gier z gazet i magiczne instrukcje POKE które wraz z BASICową logiką GOTO tworzyły idealną kombinację do jednorazowego programowania:)

Z punktu widzenia rynku, to bardzo ciekawy moment – mamy bardzo sfragmentowane produkty, których wspólnym mianownikiem jest BASIC, często niekompatybilny, i gdzie jeszcze nikt nie jest przygotowany by płacić za software.

### IBM PC (1981) czyli Imperium kontratakuje szesnastoma bitami.

W 1981 Big Blue wreszcie się ruszył na rynku mikrokomputerów i zaczął z grubej, jak na owe czasy rury, czyli z IBM PC opartym o 16-bitowy Intel 8086 (1978). To jest właśnie ta „nasza” architektura która dziś w formie niezmienionej dotrwała w mechanice BIOSu (jeśli ma się wyłączone UEFI). IBM PC wbrew nazwie (personal computer) był raczej kierowany do użytkowników komercyjnych, jak stacja do pracy (a nie do zabawy i hobbystów jak home computer).

IBM PC był sprzedawany z BASICiem z Microsoftu wgranym w ROM oraz system MS-DOS, który był ulepszoną wersją znanego z 8-bitowców systemu CP/M © Digital Research zaimplementowany przez Tima Patersona ze Seattle, który sprzedał swój system Billowi Gatesowi za 75 tyś dolarów (750tyś na dzisiejsze patrząc na ceny nieruchomości w okolicy).

Pecet był komercyjnym sukcesem między innymi przez to, że IBM w zasadzie nic w nim nie opatentował, więc zaczęli masowo go kopiować tańsi poddostawcy zarówno z USA jak i z Dalekiego Wschodu. To pierwszy ważny krok w układance pt. dlaczego jest jak jest? Ponieważ wszyscy mogli kopiować IBM, wszyscy zaczęli go kopiować i nie opłacało się kopiować kogoś innego skoro wszyscy kopiują IBM. Bo konkurencyjny paradygmat reprezentowany na rynku przez ukochane dziecko hippisów i hipsterów, czyli Apple, kopiować było dużo trudniej.

Historia Apple jest ujęta w więcej filmów niż cała pozostała historia komputerów razem wzięta, więc nie będziemy to rozwodzić się nad geniuszem Woźniaka, nad arogancją i boską iskrą u Jobsa i nad tym kto był po ciemnej stronie mocy. Istotne jest to, że Apple wybrał 16-bitową Motorolę 68K jak procesor i postawił w swoim Macintoshu na interfejs graficzny. _„Mieliśmy bogatego sąsiada na osiedlu, to był Xerox PARC, więc kiedy Jobs wyniósł od niego magnetowid, to niech się nie czepia, że ja podwędziłem stereo”_ – zmyślony cytat z Billa Gatesa, ale fabuła jest prawdziwa: Xerox pławiący się w gotówce z powodu sukcesu kserokopiarek dał wolną rękę paru łebskim gościom w Palo Alto i oni zaprojektowali jak by powinien funkcjonować komputer za 10 lat jeśli prawo Moore’a się utrzyma więc nie musimy się liczyć z teraźniejszymi kosztami. Xerox Alto, bo o nim mowa został zaprojektowany w 1973: z myszką, laserową drukarką, GUI, połączeniem do sieci, kamerką, etc. Taki prototypowy proof-of-concept, którym zainspirował się zarówno Steve Jobs i Bill Gates.

Apple Macintosh trafił na rynek w 1984r. Trzeba Jobsowi przyznać, że zmusił swoich ludzi to tego, by przekształcili dosyć koślawy prototyp Xeroxa w bardzo zgrabny i przemyślany komputer. Owszem, przegrzewał się, ale taki był fetysz Steve’a Jobsa. Nie był też przystosowany do rozbudowy, ale nie oszukujmy się, nawet wtedy większość ludzi nie rozbudowywała swoich komputerów. Tylko niestety po prostu okazał się być zbyt drogi. I zbyt zamknięty na naśladowców.

Klony IBMu produkcji tych wszystkich zapomnianych już firm jak NEC, Gateway, Compaq etc… były w 100% kompatybilne z oryginalnym PC. Można było przepinać karty rozszerzeń, przenosić oprogramowanie etc. Szybko okazało się, że część pomysłów z klonów wygrywa z oryginalnym designem IBMa (przycisk turbo, karta Hercules) i zostaje wtórnie zaadoptowana przez kolosa.
Z Applem było inaczej – producenci klonowali nie fizyczny produkt a cały ekosystem. I tu trzeba wspomnieć o dwóch: Atari ST (1986) zaprojektowane prze Jacka Trzmiela oraz Amiga 1000 od Commodore. Oba posiadały własne graficzne systemy operacyjne oraz używały Motorolii 68K (która obiektywnie była wówczas lepsza od procesorów Intela).

IBM w tym czasie wypuszczał kolejne serie: PC XT (1983) oparte o Intel 8088, oraz PC AT (1987) oparty o Intel 80286, technicznie ciągle odbiegające od produktów konkurencji.

I tu wraca na scenę Microsoft wypuszczając w 1985r okienkową nakładkę Windows. Zarówno wizualnie jaki użytkowo był Windows dużo do tyłu za systemem Apple’a ale był dostępny na największy z rynków – rynek PC. Niech nas nie zmyli nasza pamięć, Norton Commander pojawił się dopiero w 1986.

Microsoft po sukcesie pierwszego PC pracował razem z IBM nad systemem OS/2. Miała to być nowoczesna, wielozadaniowa i graficzna opowiedz IBM na konkurencję ze strony Apple/Atari/Commodore. Co poszło nie tak więc? Wbrew pozorom nie był to spisek i sabotaż biednego korporacyjnego Goliata (IBM) ze strony złego softwareowego Dawida Gatesa. Po prostu IBM naobiecywał klientom przysłowiowe cuda na kiju, między innymi to, że nowy system będzie wspierał wielozadaniowość na komputerach PC AT (czyli Intel 80286).

### Procesory i OS/2

Tu musi nastąpić dłuższa dygresja, by zrozumieć co tak naprawdę się stało i dlaczego pewnych rzeczy nie dało się zrobić tak, jak korporacyjny management IBM sobie zaplanował (na podstawie optymistycznych raportów przygotowywanych przez zespoły liczące na awans).
Zacznijmy od krótkiej historii procesorów Intela. Wszystkie 16-bitowe i wzwyż procesory były kompatybilne wstecznie. Tryb pracy procesorów 8086 (1978) oraz 8088 (1979) nazywam trybem rzeczywistym. Wspiera on maksymalnie 1MB RAM. Wraz z wprowadzeniem 80286 (1982) wielkość dostępnej pamięci rośnie do 16MB (ale dostęp do wszystkiego powyżej 640KB to jest cyrk zwany EMS/XMS) i co najważniejsze pojawia się tryb chroniony. Najważniejszą jego funkcjonalnością jest izolacja pamięci między procesami i preemptive multitasking. Tyle, że procesor 286 był taką trochę niedoróbką przed w pełni 32-bitowym 80386 (1985) którą się dużo trudniej programuje niż jego młodszego brata.

Motorola swój procesor 68K (hybrydowy 16/32 bity) z wsparciem dla wielozadaniowości wypuściła już w 1979r, tuż po 8086. Ale ponieważ IBM zadecydował się używać procesorów Intela wielozadaniowy OS/2 nie mógł powstać od razu, był dużo bardziej skomplikowany, oraz dopiero trzecia generacja procesorów Intela umożliwiła pecetom rozwinięcie skrzydeł. Poza tym OS/2 był niejako z definicji skazany na second-system effect czyli bardzo częsty problem kiedy to zespół inżynierów po pierwszym working prototype tworzy system gdzie wrzuca wszystkie swoje wymarzone rozwiązania… i tego się nie da zrobić. Czymś takim był Netscape 6.0, Windows Vista czy nigdy nie powstały następca Systemu 7 Apple.

Microsoft widział, że GUI to jest kluczowy element ekspansji na rynku komputerów i nie może czekać na świętego nigdy zanim wreszcie Intel i IBM wypuszczą odpowiednią maszynę, by spełniała wszystkie wymogi inżynierów, sprzedawców i klientów. Mało znanym faktem jest to, że Microsoft dostał propozycję napisania Windows na Atari ST, ale projekt nie doszedł do skutku z powodu zderzenia egotystycznych osobowości wścieli obu firm. Atari, kopiując zamknięty ekosytem Apple, popełniało te same błędy, ale ich komputery były pod względem stosunku mocy do ceny najlepszym wyborem na rynku, więc gdyby wtedy Jack Tramiel (który wcześniej pokłócił się z zarządem Commodore i z niego odszedł) miał trochę więcej wyrozumiałości dla Gatesa to kto wie jak by się to potoczyło.

> Dla porządku dodam, że Atari ST było bardzo długo popularne wśród twórców muzyki elektronicznej (bo miało dobry chipset do MIDI) a Amiga z powodu dobrego chipsetu graficznego była dobrą platformą do (tworzenia) gier.

OS/2 w pierwszej tekstowej wersji wyszedł dopiero w 1987r i wraz z Minixem pozostają jedynymi systemami dedykowanymi 80286. Nakładka graficzna (Presentation Manager) wyszła dopiero w 1988, równolegle do Windowsa 2.1. Pierwsze Windowsy były fatalne, bez żadnej poważnej wielozadaniowości – ale były. A OS/2 przegapił swoje okienko, choć IBM inwestował w niego aż do drugiej połowy lat ‘90tych

### Przełom 1990 czyli piłka w grze

Przypomnijmy krótko jak wygląda świat oprogramowania konsumenckiego przełomu 89/90:

* Jest już Norton Commander (którego rola przez dzisiejszych użytkowników jest niedoceniana/zapomniana)
* W 1990 wychodzi Windows 3.0 pierwszy Windows z wielozadaniowością na poważnie (na 80386)
* AMD w 1991 wypuszcza dużo tańszy (i w sumie piracki) klon 80386, który w wersji AMD 386DX 40Mhz istotnie przeskoczy to co Intel miał do zaoferowania
* Amiga ma swoją Amigę 1000 i budżetową Amigę 500 z graficznym Amiga OS
* Jest Atari ST z niestabilnym systemem GEM
* Apple ma już cztery kolejne modele Macintosha, a w 1991 wychodzi System 7, czyli podstawa wszystkich „klasycznych” Macintoshów która ucementowała pozycję Macintosha ma rynku wydawniczo-graficznym, wraz z wydanym w 1987 Photoshopem.
* Microsoft od 1985 ma Worda, a od 1989 Worda na Windowsa. Excel to 1987r. Windows 3.1 czyli poprawiona wersja dobrego graficznie Windowsa 3.0 to rok 1992
* Standardem biurowym ciągle pozostaje Lotus 1-2-3 (1983) i WordPerfect ( 1982 ).

**W świecie niedorobionych klonów IBM PC, niestabilnego Windowsa 3.0 i drogiej (Apple) lub taniej (Amiga) konkurencji nie jest oczywiste, że postawienie przez Microsoft na co prawda największy rynek (PC) ale technicznie zacofany jest dobrym wyborem**. Znowu kłania się nieskonsumowany romans _Trzmiela_ z _Bramą_.

Dlaczego więc udało się Microsoftowi nie tylko utrzymać pozycję, ale doprowadzić do sytuacji, że w 1995r jednym ruchem (Windows 95) udało się pozamiatać cały rynek komputerów użytkowych?
Pierwszy klucz już mamy: otwartość architektury IBMa. Drugim kluczem jest piractwo na tej największej z platform, które choć nie generowało profitów dla Microsoftu to generowało rynek. A trzecim elementem, o którym nikt nie pamięta, są narzędzia developerskie.

## Krótka historia (o)programowania

Pierwszym językiem był asembler symboliczny (czyli instrukcje MOV/JMP zamiast kodów binarnych). Drugim – używany do dziś Fortran (1957) oparty o Speedcoding (1953). Zarówno Speedcoding, FlowMatic (1955, dał początek Cobolowi) czy Plankalkul Zussego (1942-45) oraz Superplan (1952) to były bardziej konstrukcje teoretyczne niż rzeczywiste języki. Równolegle z Fortranem powstaje Cobol (1959) oraz Lisp (1958). Fortran miał być dla inżynierów, Cobol dla biznesu a Lisp dla cybernetyków od sztucznej inteligencji. Jak dołączymy do tego jeszcze Sketchpad (1963) Sutherlanda to mamy w zasadzie wszystkie współczesne paradygmaty programowania dostępne już wtedy.

O ile Lisp przetrwał w niezmienionej formie do dziś, a Cobol już w latach ’90 był głównie źródłem memów, to Fortran – będący najbardziej praktycznym z tych języków – ma niezliczone potomstwo do dziś, łącznie z niesławną instrukcją GOTO, tablicami oraz numerowaniem linii. Ale był językiem nastawionym na to by pisać najbardziej wydajne programy trochę prościej niż assembler. W odróżnieniu od Lispa i Cobola, które były miały oszczędzać czas programisty zamiast czasu procesora.

Europejską odpowiedzią na Fortrana był Algol (1958) na którego składni potem wzorował się Pascal, Simula oraz C. Język dużo bardziej przejrzysty i łatwiejszy do czytania a wciąż nadający się do implementowania wydajnych algorytmów numerycznych.

Dla nieprzyzwoitości wspomnimy tu jeszcze o BASICu (1960) który przejął i uprościł składnie Fortrana zostawiając jego podstawowy paradygmat: czyli, że program to jest ciąg ponumerowanych linii.

Fortran i Basic, a także do pewnego stopnia Lisp oraz Algol mają trzy wspólne cechy:

1. Nie mają żadnych wbudowanych mechanizmów abstrakcji danych. Najbardziej skomplikowanym typem jest dla nich zawsze tablica liczb. Tu Algol pokazuje już nową generację wraz z rekordami
2. Jedynym mechanizmem abstrakcji kodu są procedury (oraz GOTO) co z brakiem mechanizmu abstrakcji danych praktycznie uniemożliwia modułowość na innym poziomie niż binarnym.
3. I dodatkowo całe zarządzanie pamięcią jest ręczne. Owszem Lisp miał odśmiecanie i automatyczne zarządzanie pamięcią już od 1959r, ale bardzo długo był to jedyny język z tą funkcjonalnością. Ciekawostką jest, że **Basic miał GC dla zmiennych tekstowych – i to jeden z powodów dla których stał się taki popularny**.

O ile wszyscy powiedzą, że bez C i pointerów nie ma programisty, to jednak przyzwyczajeni jesteśmy choćby do rekordów struct które umożliwiają jakieś łączenie heterogonicznych typów razem. Bez tego mamy w zasadzie niepowiązane zmienne które bardzo łatwo pomieszać przez przejęzyczenie. Np. w Basicu każdy nieużyty ciąg znaków jest z automatu zmienną liczbową o wartości 0. Czyli jedna literówka w warunku IF NOT i warunek jest zawsze spełniony (0 to false, -1 to true).

Dlatego takim przełomem był Pascal Nikolasa Wirtha (1970) i C (1972). I o ile dziś jeśli ktoś Pascala kojarzy, to pewnie z nieudanych lekcji z liceum, to był to istotny przełom w dostępności programowania. Program napisany w Pascalu da się zrozumieć po przeczytaniu:) Zwłaszcza Turbo Pascal (1983, np. Norton Commander był napisany w Turbo Pascalu) od Borland stworzony przez Andres Hejlsberga (twórcę Delphi, C# oraz TypeScript !!! – widać jak wiele jeden łebski facet znaczy), bo Turbo Pascal był dodatkowo bardzo szybkim kompilatorem. Jeszcze w podręcznikach z wczesnych lat ’90 to rozróżnienie na języki imperatywne i kompilatory jest kluczowe, bo o ile kompilowane programy działały dużo szybciej to kompilacja trwała kilka-kilkanaście minut, co bardzo obniżało wydajność programisty w czasach gdy bez StackOverflow trzeba było wszystko rozwiązywać metodą prób i błędów. Przewagą C nad Pascalem nie jest składnia, ale bardziej bezpośredni model zarządzania pamięcią (void**) który wraz z pseudomaszynowymi instrukcjami jak i++ umożliwia lepszą kontrolę nad wykonywalnym kodem. Oraz, co najważniejsze, C ma jednostki kompilacji (pliki .h i .c) więc trochę łatwiej zarządza się większym projektem.

Programowanie na x86 w C czy Pascalu jest uciążliwe, ale programowanie GUI to mordęga. Mordęga wtedy i mordęga dziś w JavaScripcie. Dlatego – wracając do przełomu rok 1990 – żeby system operacyjny z graficznym interfejsem użytkownika rozwinął skrzydła, potrzebny był język programowania, w którym tworzenie UI nie będzie aż takim koszmarem. W Windowsie był to Visual Basic (1991). Wiem, proszę się nie śmiać, ale tak właśnie było. Visual Basic, czyli rozwinięcie QuickBasica to był ciągle ten okropny Basic ale dzięki temu, że był imperatywny (krótkie iteracje), dzięki wbudowanemu GC, czy wreszcie dzięki mieszanemu obiektowo-imperatywnemu stylowi programowania był właśnie tym brakującym elementem układanki zapewniającym sukces Windowsowi. Nakład pracy na stworzenie dziurawej aplikacji okienkowej w Visual Basicu był nieporównywalnie mniejszy niż w C czy C++ (MFC/Win32s m_lpsz i notacja węgierska:)).

### Obiekty otwierają okna

Absolwenci AGH kojarzą pewnie SmallTalk (1980), czyli pierwszy dynamiczny obiektowy język programowania, protoplastę Rubiego. W zasadzie jedyną jego funkcją było tworzenie UI, bo jako piewszy język miał obiektowy framework do tworzenia interfejsów graficznych. Programowanie obiektowie jest starsze niż SmallTalk, bo zaczęło się w od Simuli (1962), od której model statycznie typowanych obiektów przejął C++ (1985). Równolegle powstało Objective-C (1984) czyli poszerzenie C o mechanizmy obiektowe ale używając paradygmatu SmallTalka (dynamicznego). Jeśli ktoś nie zna, to polecam porównać sobie te dwa języki, niby to samo obiektowe C, a jednak różnica w podejściu zwala z nóg. Objective-C jest ważne, bo powstała w 1985 firma NeXT założona przez Steva Jobsa użyła go to stworzenia własnego okienkowego, obiektowego systemu operacyjnego (NeXT Step, który jest protoplastą, wraz z FreeBSD systemu MacOSX). Jobs w 1985r zauważył, że sensowny język programowania jest niezbędny do dalszego rozwoju okienkowych systemów. Amiga i Atari tego nie zauważyły i przepadły. Apple uratował się przyjmując Jobsa z powrotem, a Unixy choć standard X11 powstał w 1984 w zasadzie nie dorobiły się jednolitego, sensownego API okienkowego do 1996r kiedy powstało Qt/KDE (zmodyfikowane C++) czy Gtk/Gnome (1999, niby w C ale z ręcznie zaimplementowanym obiektami GObject).

Objective-C jest zdecydowanie ładniejszym językiem niż Visual Basic, ale miał jeden drobny defekt. Jego półautomatyczny manager pamięci (instrukcje release/retain) działał tylko w obrębie event loop który początkowo obsługiwał tylko kooperacyjną wielozadaniowość – co z resztą jest typowe dla wczesnych systemów okienkowych. Windows dopiero od wersji 3.0 nauczył się przełączać zadania inaczej niż po dobroci, a dopiero Windows 95 potrafił zabijać złośliwie procesy.

Warto też wspomnieć o tekstowej bibliotece okienkowej od Borlanda, czyli Turbo Vision (1990), która bardzo długo w Polsce była standardem tworzenia aplikacji biurowych (Windows się u nas wolno przyjmował, za dużo było tekstowych monitorów Herculesa). Z TurboVision powstało w 1995r Delphi, które przejęło od Visual Basic pałeczkę pierwszeństwa w tworzeniu prostych aplikacji pod Windowsa, aż do C# (2000).

Gdy dziś patrzymy na obiekty w programowaniu, zwłaszcza w backendzie, gdy przeszliśmy jakąś uczelnianą ścieżkę rozwoju w C++/Javie, to widzimy, że coś jest nie tak. Że to jest przerost formy nad treścią i że te obiekty nijak mają się do tego jakbyśmy chcieli nasz program pisać. Nic dziwnego, poza UI obiekt często są archaizmem. W C++ dużo więcej algorytmów jest prościej wyrazić w szablonach niż w klasach. Bo z jednej strony mechanizm obiektów w klasycznym C++ jest dosyć ubogi (model ObjectiveC jest trochę wygodniejszy), a z drugiej strony kiedy mamy ```List<T>``` to jest to dużo wygodniejsze (i szybsze) niż ```List``` gdzie każdorazowo trzeba dynamicznie odpakować elementy. Generyki dają **statyczną kontrolę typów, czyli wykrywają błędy wcześniej, zanim program zostanie uruchomiony, co zawsze w długim terminie jest lepsze**. Tyle, że w C++ obiekty to rok 1985, a szablony to 1989 (de facto nieustandaryzowane jeszcze parę lat później).

## Wielka Konwergencja (Millenium Edition)

Gdzieś na przełomie milleniów rozpoczęła się Wielka Konwergencja Architektur i zanik wszystkich niemainstreamowych rozwiązań:

* SGI z procesorami MIPS do 2002 (miałem kiedyś wygrzebany ze śmietnika SGI Indy, piękny design)
procesory DEC Alpha, głownie HP & Compaq wygaszenie 2004-2007
* HP-PA RISC HP-UX wygaszenie 2008
* Sun i UltraSPARC, wygaszenie w 1997
* POWERPC (IBM, Apple, Motorola) wygaszanie: Apple (2006), potem jeszcze xbox 360 i ps3

Wszystkie te architektury to klasyczne RISC, czyli przeciwieństwo Intelowych x86 CISCów. Zostało IA64 (symbolicznie), amd64 i ARM. Motorola 68K odpadła wcześniej, gdy Apple przeszło na PowerPC.
Równolegle w 2001r Microsoft wypuścił Windows XP, a Apple MacOSX. To są już zupełnie nowoczesne system operacyjne, paradoksalnie oparte podobne założenia mikrojądra sworzone dla VMS przez Dave’a Cutlera. Dwie dekady póżniej, kiedy Apple zdążył mieć romans z Intelem i się z nim rozstać, a Windows ma wbudowane Ubuntu tamten fragmentaryczny świat końcówki lat ’90 gdy samych środowisk okienkowych na Unixa było więcej niż kilka, kiedy dogorywała Amiga i kiedy C/C++ było synonimem programowania wydaje się być Dzikim Zachodem, gdzie jeszcze wszystko było możliwe.

## Dziś

Co ciekawe, choć powstały fantastyczne modele programowania (Haskell, Ocaml, TypeScript, Erlang) to ciągle programowanie UI jest kopaniem się z koniem. Programowanie współbieżne ciągle dylematem między szybkim, dynamicznym i eleganckim Erlangiem. A uniwersalnym choć strasznie nieodpornym na błędy modelem z C. Równoległego Ocamla nie doczekaliśmy się dopiero 2025. Do tego mamy kilka ezoterycznych języków jak Zig czy Pony, oraz ambitną próbę zastąpienia C przez RUSTa.

Poza tym zmienił się assembler: dziś między REP MOVSW a tym co rzeczywiście wykona rdzeń procesora jest całkiem spora przepaść i poza Andurino i wiekowymi mikrokontrolerami to nikt się w takie rzeczy nie bawi. Chyba, że akurat uczy się jak używać CUDA.

Wszystkie kompilatory zaczęły używa LLVM i prawie nie używa się już dziś natywnym binarek, wszystko przechodzi przez byte-code i JIT. Python ze swoją składnią jak porcja speedballa opanowuje kolejne segmenty rynku i tylko kompilując pakiet pip ze źródeł widzimy jak kod w Fortranie staje się BLASem, przez bindingi C łączy się z CUDA i ostatecznie magicznie zarządza nim Keras chowający pod spodem Tensorflow.

## Posłowie

Bo artykuł ten ma dwa źródła – pierwsze to historia zabawy z DosBox i przypominanie sobie jak kiedyś najmniejsze elementy abstrakcji, jak pętla WHILE zbudowana z GOTO do czytania DATA potrafiła cieszyć. Bo kiedyś język to był groch z kapustą i BASIC ma instrukcje zarówno zwykłe, jak i do generowania dźwięków, grafiki i plików. Wszystko na tym samym poziomie abstrakcji co GOTO – i wszystko w nim było pod górkę
Drugie to pytanie – jak zachęcić dzieci do programowania? To drugie jest oczywiście dużo ogólniejsze, ale metodologia nie jest oczywista, bo mamy tu przynajmniej trzy równorzędne drogi:

1. Interakcje, czyli żółwik z LOGO – nic tak nie cieszy jak zrobienie czegoś. Np. gry w którą można ogrywać znajomych
2. Algorytmika klasyczna, czyli olimpiada. W zasadzie najbardziej praktyczna scieżka, bo model rekrutacji w informatyce sprowadził się przez Google do algorytmiki. I serwisów szlifujących algorytmiczny pazur jest całe mnóstwo (od najstarszego z Uniwersytetu z Valladoild, przez TopCoder, LeetCode czy polske Sphere czy Codility)
3. Oraz klasyczny Computer Science czyli piękno programowania funkcyjnego, monad, algebry abstrakcyjnej i rozwiązywania problemów w jednej linijce. Jeśli się ona kompiluje to jest wtedy sama automatycznym dowodem, że algorytm jest poprawny. Wiedza kompletni bezużyteczna, czysta sztuka dla sztuki.

## Koniec pewnego rozdziału: AI i programowanie

Rewolucja dużych modeli językowych (LLMów) z 2022 pokazała, że AI może być naprawdę dobre w programowaniu. Powiedzmy tryb agentowy/interaktywny (z automatyczną pentlą zwrotną jak AI zrobić coś źle) działa dużo lepiej (Cursor, GitHub Copilot) niż tryb generatywny (ChatGPT) i widać, że AI ciągle momentami zmyśla i dopiero interakcja z kompiltatorem/testami pozwala mu wyeliminować większość pomyłek. Tzn AI jest w stanie rozwiązać zadania z LeetCode lepiej niż ktokolwiek (poza Psycho:)) i rzeczywiście z perspektywy pojedynczego programisty pracującego nad relatywnie prostym projetem jest to przełom i niesamowite przyśpieszenie. *Vibe Coding*, jak to nazwał Andrej Karpathy, działa i przynajmniej w kwestii prototypowania eliminuje mnóstwo roboczogodzin.

Tu pozwole sobie na dygresję filozoficzną: programowanie od początku historii to skupia się na eliminowaniu roboczogodzin programistów. Gdy używasz STLa, gdy używasz GUI buildera albo bilblotek numerycznych to eliminujesz więcej roboczogodzin pracy niż życie jednego człowieka. Stąd AI jest po prostu kolejnym, robiącym wielkie wrażenie, elementem historycznego ciągu postępu w informatyce, jakim jest głównie eliminowanie roboczogodzin informatyków. Jak ktoś uważa, że jest inaczej niech spróbuje napisać cokolwiek nowoczesnego w GW/Turbo/QuickBasicu. Najlepiej z dostępem do sieci i interfejsem graficznym:) 

W czym Copilot jest dobry? W generowaniu kodu który rozwiązuje prosty (najczęściej wcześniej udokumentowany na StackOverflow) problem. W czym nie jest taki dobry? W ograniczeniu złożoności: autogenerowany kod jest często przegadany, zbyt skomplikowany na zadany problem i każda próba dołożenia funkcjonalności/korygowania kursu czy jak wspomniałem na początku artykułu: werbalizacji/formalizacji oczekiwań użytkownika/klienta powoduje istotny wzrost złożoności i trzeba z jednej strony wiedzieć co się chce uzyskać, z drugiej dawać AI konkretne rady, z trzeciej wykazać się dużą samodyscypliną by redukować te nadmierną złożoność. Przynajmniej na razie:) 

Co sprawia, że AI programuje lepiej? Krótszy cykl interakcji dla Agenta. Im szybciej do AI dociera, że to co robi jest bez sensu, tym mniej popełnia błędów i mniej się samo wyprowadza w pole. A poza tym dla AI jest generalnie wszystko jedno w jakim języku ma programować, wszystkich uczy się bardzo szybko. Czyli tu na białym koniu wjeżdżają języki trudne, jak Ocaml, Haskell, Rust, bo dla generowania kodu to bez znaczenia czy to kod w prostym Pythonie, czy mocno przegadany asercjami kod w Rust. Przy czym ten kod w Pythonie może się okazać błędny dużo później, a kod w Rust czy Haskellu, gdy będzie źle napisany po prostu się nie skompiluje.

Czyli **czeka nas era języków silnie statycznie typowanych z możliwie jak największą kontrolą poprawnośći na etapie kompilacji**, złota era dla programowania funkcyjnego i innych ezoterycznych języków, gdzie dotychczas problem była bariera wejścia. Copilot Agent nie boi się takich trudności:) Kto wie, może nawet przemigruje nasz dotychczasowy _codebase_ z JavaScript na TypeScript i z C++ na Rust?

