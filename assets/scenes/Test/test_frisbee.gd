extends Spatial


func _ready():
	$EarthDisc.state = $EarthDisc.DiscState.CONTROLLED
	$EarthDisc.launch(5 * Vector3(0.25, 0.5, -1))

func _process(delta):
	if $EarthDisc.transform.origin.y <= 0:
		pass #$EarthDisc.visible = false
