# CommonSelection

简单的双选，可以在editor里面填上选择true和false分别的字符串，方向键左显示true string 右显示false string，这两个是富文本，可以做到美术要求的效果。

Description String可以填上面一列的描述，默认后面会跟一个时间参数，不需要不写{0}就可以了。

在选择完成之后会调用OnSelection的Event

在进入这个页面的时候会把GameManager上的inputaction关掉，用canvas上面的，enter之后canvas会关掉。