extends Node

export var punch_start_vel: float
export var punch_end_vel: float

var hand: ARVRController

enum PunchState {
	INVALID,
	NONE,
	PUNCHING
}
var state: int

signal punched(vector)

var _start: Vector3
var _end: Vector3
var _vel: Array

func _ready():
	var parent: Node = get_parent()
	if typeof(parent) == TYPE_OBJECT and parent.is_class("ARVRController"):
		state = PunchState.NONE
		hand = parent
	else:
		state = PunchState.INVALID
		hand = null


func _physics_process(_delta):
	match state:
		PunchState.NONE: 
			if hand.get_linear_velocity().length() >= punch_start_vel:
				state = PunchState.PUNCHING
				_start = hand.get_transform().origin
				_vel = []
		PunchState.PUNCHING:
			if hand.get_linear_velocity().length() <= punch_end_vel:
				state = PunchState.NONE
				_end = hand.get_transform().origin
				
				# Calculate average velocity
				var vel: float = 0
				if len(_vel) > 0:
					for v in _vel:
						vel += v
					vel /= len(_vel)
				
				# Notify parent
				emit_signal("punched", (_end - _start) * vel)
			else:
				_vel.append(hand.get_linear_velocity().length())
