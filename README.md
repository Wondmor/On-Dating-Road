# On-Dating-Road
BOOOM Game Creation Competition Entries

鉴于咱们是全球分布开发，沟通未必及时。为免节外生枝，尽量做到各部分资源代码独立。

## 资源说明
* Levels：下属每个文件夹对应一个Scene，包含其相关所有资源。
  * TestMain：测试用分发Scene，方便build完集中测试。加了新Scene把第一个Button复制下改下按钮文本就行。
  * TrashShooting：踢球+垃圾分类小游戏。
  * GameStart：设置GameLogicManager，用于进入正规游戏流程。
  * Prelude：开场剧情引入，出logo。
  * Bridge：各游戏之间的过桥部分。
  * CoinSkill：处理硬币技能阶段。
  * Shopping：购物阶段。
  * Ending：结局。

## 主线使用说明
GameLogicManager：全局单件，控制游戏流程跳转。

### 主线负责什么
主线流程。小游戏前主角会扔完硬币，小游戏后主角会继续扔硬币（或者进下阶段结算）。
强制金钱、人品、剩余时间不能为负数。

### 主线不负责什么
每个小游戏之前的npc交流、询问玩家是否跳过游戏。

### 读取数据
~~~
var money = GameLogicManager.Instance.GameData.money; //金钱
var positiveComment = GameLogicManager.Instance.GameData.positiveComment; //好评人数
var countDown = GameLogicManager.Instance.GameData.countDown; //剩余时间
~~~

### 跳过游戏或游戏结束
先按需清掉DontDestroy，然后调用以下任一API。

~~~
//玩家跳过小游戏。按默认参数扣除好评人数（人品值）。之后跳转到主线scene继续游戏流程。
public void OnMiniGameRefused();

//玩家完成小游戏。赋值金钱、好评人数和时间。之后跳转到主线scene继续游戏流程。
public void OnMiniGameFinished(float money, float positiveComment, float countDown);

//玩家完成小游戏。赋值金钱和好评人数，按默认参数扣除时间。之后跳转到主线scene继续游戏流程。
public void OnMiniGameFinished(float money, float positiveComment);

//e.g.
 GameLogicManager.Instance.OnMiniGameFinished(
     GameLogicManager.Instance.GameData.money + 5.5f,
     GameLogicManager.Instance.GameData.positiveComment + 20.0f,
     GameLogicManager.Instance.GameData.countDown - GameLogicManager.c_StandardGameDuration * 1.1f);
~~~
    
### 数值部分

#### 分级

玩家好评和金钱都分为5级，第5级为完美游戏，前4级是正常游戏

#### 总数
期望（玩家4级完成）
金钱： 80
好评： 96

#### 分配
| 游戏     | 钱  | 好评 |
| -------- | --- | ---- |
| 外卖     | 30  | 30   |
| 公车     | 20  | 40   |
| 捡垃圾   | 20  | 40   |
| 共享单车 | 20  | 0    |
| 串       | 10  | 20   |

#### 比例
从5级开始分别比例为
| 5      | 4    | 3   | 2   | 1   |
| ------ | ---- | --- | --- | --- |
| 无限制 | 100% | 80% | 50% | 10% |
