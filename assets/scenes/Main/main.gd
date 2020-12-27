extends Spatial

var disc


func _ready():
	vr.initialize()
	
	disc = load("res://assets/objects/EarthDisc/EarthDisc.tscn").instance()
	add_child(disc)
	disc.global_transform.origin = Vector3(-1, 1, 0)
	disc.scale = Vector3(0.1, 0.1, 0.1)
	disc.latch($Player/OQ_RightController)
	
	$EarthDisc.latch($Player/OQ_RightController)


func _process(delta):
	$Marker.global_transform.origin = \
		$EarthDisc.get_global_transform().origin \
		+ 0.1 * $Player/OQ_RightController.get_palm_transform().basis.x


func _on_FunctionPunch_punched(vector):
	print("Punch Detected")
	disc.global_transform.origin = \
			$Player/OQ_RightController.get_global_transform().origin
	disc.launch(2 * vector)
	
	disc = load("res://EarthDisc.tscn").instance()
	add_child(disc)
	disc.global_transform.origin = Vector3(-1, 1, 0)
	disc.scale = Vector3(0.1, 0.1, 0.1)
	disc.latch($Player/OQ_RightController)
