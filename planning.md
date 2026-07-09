## Attributes

**player attributes**
LastName
FirstName

**batting attributes**
Concentration
Aggression
BattingStrength
BattingWeakness

**bowling attributes**
pace
accuracy
StockBall
VariationBall

**fielding attributes**
catching
fielding
Keeping
Captaincy

**general attributes**
fitness
health (fitness based modifier)
BattingOrder


## Bowling inputs

**target (main bowler input)**

s   7 | 8 | 9
    ---------
l   4 | 5 | 6
    ---------
w   1 | 2 | 3

    w   o   l

shortBall 7,8,9
lengthBall 4,5,6
fullBall 1,2,3
wideBall 1,4,7
offBall 2,5,8
legBall 3,6,9

9 shortAndLeg
8 shortAndOff
7 shortAndWide

6 lengthAndLeg
5 lengthAndOff
4 lengthAndWide

3 fullAndLeg
2 fullAndOff
1 fullAndWide

**computer player randomisation**
default to 5
rnd(2) 2,4,6,8
rnd(6) 1,7
rnd(30) 3,9

**additional bowler inputs - see bowling modifiers**
effort (bool)
variation (bool) uses default variation from player

**pace**
value, enum, label, description 
9, fast, "Fast", 150 and above
8, fastmedium, "Fast-medium", 140-149
7, mediumfast, "Medium-fast", 130-139
6, medium, "Medium", 120-129
5, slow, "Slow", 110-119
4, slowerball, "Slower ball", 100-109
3, spinerquick, "Spinner (quick)", 90-99
2, spinnermedium, "Spinner (medium)", 80-89
1, spinnerslow, "Spinner (slow)", 70-79
0, dead

## Bowling modifiers

**health - see also batting modifiers**
decrease x for every over
multiply effect by 1.5 for consecutive effort balls
increase x for every 5 overs not bowled
multiply effect by 2 for drinks, 3 for tea, 4 for lunch

**variation**
affects movement, increased dismissal with battingweakness

value, enum, label, effect, description
9 straightBall, "Straight", no movement
8 inswinger to leg, fullBall movement++, lengthBall movement+  
7 outswinger no off, fullBall movement++, length movement+
6 offcutter to leg, fullBall movement+, length movement++
5 legcutter to off, fullBall movement+, length movement++
4 offspinner to leg, fullBall movement+, length movement++
3 legspinner to off, fullBall movement+, length movement++
2 armBall to off, fullBall movement+, length movement++ needs to differ from legspinner
1 googly to leg, fullBall movement+, length movement++ needs to differ from offspinner
0 reserved

where does slowerBall sit?

**ball condition - for later implementation**
Random event can affect ball condition, can ask for ball to be replaced (50% chance)

9 0 overs swing++, pace++
8 10 overs swing+, pace+ 
7 20 overs pace+
6 30 overs no effect
5 40 overs spin+
4 50 overs spin++
3 60 overs spin++, pace-
2 70 overs sping+++, pace-
1 80 overs sping+++, pace--
0 unusable

**pitch condition - for later implementation**

9 wet ballcondition-
8 green pace++
7 green pace+
6 normal no effect
5 normal movement+ (pace 1-3 only)
4 dry pace- (pace 6-9 only), movement+ (pace 1-3 only)
3 dry pace- (pace 6-9 only), movement++ (pace 1-3 only)
2 dusty pace-- (pace 6-9 only), movement++ (pace 1-3 only)
1 crumbly ballcondition-, pace--- (pace 6-9 only) movement+++ (pace 1-3 only)
0 unuseable

## Bowling outputs after modifiers
speed 1-9, line 1-9, length 1-9, movement

**line**
scoring++ if matched to BattingStrength
dismissal+ if matched to BattingWeakness 
line and length needs to align with shot selection otherwise dismissal+

9 downLeg (legGlance, hook, pull)  , risk of wide x2
8 legStump (hook)
7 middleAndLeg (pull) 
-
6 middle ()
5 middleAndOff 
4 offStump 
-
3 corridor 
2 outsideOff 
1 wideOfOff risk of wide

**length**
If matched to BattingWeakness, dismissal+

9 overhead - wide? 
8 bouncerHead scoring+(fast), scoring+++(slow) - where's the payoff?
7 bouncerBody scoring++(fast), scoring+++(slow)
-
6 shortBall scoring+(fast), scoring++(slow)
5 shortOfLength scoring+(slow)
4 halfVolley scoring+
-
3 fullBall scoring++(fast)
2 yorker,
1 fullToss, scoring-(fast), scoring+++(slow)

## Dismissals
Do we calculate a dismissal chance now which is then modified by the field?
For catches, it needs to be dependent on a fielder being in position and competent
How do we work out the mode of dismissal
Dismissal rates need to be adequately influenced by tactical skill of player

Form of Dismissal   Overall     Pace        Spin
Caught              (0.0095)    (0.0115)    (0.0078)
— Caught Behind     (0.00675)   (0.0044)    (0.0011)
— Caught Field      (0.0028)    (0.0071)    (0.0067)
Bowled              (0.0033)    (0.0035)    (0.0032)
Leg Before          (0.0024)    (0.0027)    (0.0028)
Stumped             (0.0003)    (0.0)       (0.0009)
Hit Wicket          (0.00003)   (0.000005)  (0.000002)
Run Out             (0.0005)             
Obstructing         (0.0000015) 

Chance each ball    (0.016)          (0.018)     (0.15)      

Numbers expressed for Random.Shared.NextDouble()

## Batting inputs

**Intent (main batting input)**

backFoot, crease, frontFoot
offsideShot, straightShot, legsideShot

b   7 | 8 | 9
    ---------
c   4 | 5 | 6
    ---------
f   1 | 2 | 3

    o   s   l

**BattingStrength (also stroke)**
Increases scoring
Can conflict with BattingWeakness e.g. short ball and pull
first digit corresponds to field sectors

9 legGlance
8 hook
7 pull
6 sweep
5 onDrive
4 offDrive
3 coverDrive
2 squareCut
1 lateCut

**BattingWeakness (line, length or variation)**
Increases dismissal

9 shortBall (length)
8 fullBall (length)
7 wideBall (line)
6 leg (line)
5 awayBall - outswinger, legcutter, (variation)
4 outswinger (variation)
3 
2 spinners
1 slowerball

**additional batting inputs**
attack (bool)
singles (bool) ?

attack toggle, if so will not play defensive stroke
increased likelihood of hitting in air
chance of success linked to aggression attribute

push singles increased chance of runout, 

shot modifiers
timing (and/or centred)
air/ground (bool?)

## Partially formed ideas

**weather**
reevaluate every 10 overs based on forecast

9 hot
8 overcast
7 
6
5
4
3
2
1 wet
0 abandoned

**wind**
reevaluate every 10 overs, consider how it moves around

9 northerlystrong pace+/-, clearboundary++/--
8 northerlylight clearboundary+/-
7 westerlystrong clearboundary++/--
6 westerlylight clearboundary+/-
5 calm
4 easterlystrong clearboundary++/--
3 easterlylight clearboundary+/-
2 southerlystrong pace+/-, clearboundary+/-
1 southerlylight clearboundary+/-
0 reserved

**temperature**
health modifier
capped based on location

9 >40
8 35-40
7 30-35
6 25-30 No effect
5 20-25 No effect
4 15-20 
3 10-15
2 5-10
1 <5


## Random ideas
How to implement run outs and stumpings
Generate forecast for match for weather conditions
Impact of full tosses and bouncers on noballs
Highly random events - retired hurt, injury to bowler or fielder,
   handled ball and rare forms of dismissal, skied shot

## Sundries ##
**No balls**
Quicks random 1/100
Spinners random 1/200

**Wides**
accuracy if wide (1,4,7) or leg (3,6,9)
test rate is 1 every 350 to 500 balls (quicks 200, spinners >1000)
use accuracy as the key determinant rather than pace (until testing)
need a reward payoff


## Game logic
**Bowl logic player**
read player.pace .accuracy .vitality .stockBall .variationBall
read effort target
read innings.pitchCondition .ballCondition
determine no ball
    - call noball rnd 100 (pace) rnd 200 (spin)
    - double frequency if effort ball
calculate speed
    - set speed to pace
    - calculate pitchCondition and ballCondition modifiers (omit)
    - calculate effort modifier
        - if effort, speed += 1
    - calculate health modifier
        - 1-3 speed -=2
        - 4-6 speed -=1
        - 7-9 no effect
calculate length
    - covert target to length
        - short 7,8,9 - length = 8
        - good 4,5,6 - length = 5
        - full 1,2,3 - length = 2
    - calculate accuracy and health modifier
        - Sum accuracy and health
            - 2-6 modifier 3
            - 7-11 modifier 2
            - 12-16 modifer 1
            - 17-18 modifer 0
        - Randomise fuller or shorter (positive or negative length modifier)
    - calculate length
        - Add modifiers to length
            - Cannot exceed bounds of 1-9
            - Consider possibility of >9 wide and <1 noball
calculate line
    - covert target to line
        - wide 1,4,7 - line = 2
        - off 2,5,8 - line = 5
        - leg 3,6,9 - line = 8
    - calculate accuracy and health modifier
        - Sum accuracy and health
            - 2-6 modifier 3
            - 7-11 modifier 2
            - 12-14 modifer 1
            - 15-18 modifer 0
        - Randomise fuller or shorter (positive or negative length modifier)
    - calculate length
        - Add modifiers to length
            - If >9 wide and <1 wide
            - Cannot exceed bounds of 1-9 so limit after checking for wide
calculate movement
    - to do

**Bat logic computer**



**Bat logic player**
Two of three, use other mechanism for third
Look to leg, straight, off 
Look to get forward, crease, back

**Bowl logic computer**



## Fielding
**fielding positions**
9 sectors
3 rings - 1 catching, 2 infield, 3 outfield
1 fielder per segment except keeper (mandatory) and up to 3 slips in 1.1
possible to put three behind leg and be noballed

9.3 fineLeg
9.2 shortFineLeg
9.1 legSlip
8.3 deepBackwardSquare
8.2 backwardSquare
8.1 legGully
7.3 deepSquareLeg
7.2 squareLeg
7.1 shortLeg
6.3 deepMidwicket
6.2 midwicket
6.1 shortMidwicket
5.3 longon
5.2 midOn
5.1 sillyMidOn
4.3 longOff
4.2 midOff
4.1 sillyMidOff
3.3 deepCover
3.2 cover
3.1 sillyPoint
2.3 deepPoint
2.2 point   
2.1 gully
1.3 thirdMan
1.2 flySlip
1.1 slips

## Publishing targets
**Minimal Viable Product**
- Single test match - no ground conditions
- Bowling and batting

**Addtional features**
- Match history and player statistics
- Internal team editor
- Weather
- Random events
- Commentary
