extends RigidBody

const C_LIFT: float = 1.5
const C_DRAG: float = 0.2

var hand: ARVRController
var launched: bool = false
var launch_vel: Vector3


enum DiscState {
	NONE,
	CONTROLLED,
	FLYING
}
var state: int


func _init():
	print("Creating Disc")
	state = DiscState.NONE
	hand = null


func latch(_hand: ARVRController):
	if state == DiscState.NONE:
		print("Latching")
		state = DiscState.CONTROLLED
		hand = _hand


func launch(vel: Vector3):
	if state == DiscState.CONTROLLED:
		print("Launching")
		state = DiscState.FLYING
		launch_vel = vel
		launched = false


# Modified from https://docs.godotengine.org/en/stable/tutorials/physics/rigid_body.html
func look_follow(phys_state, target: Vector3, dir: Vector3 = Vector3.FORWARD):
	var cur_dir = get_global_transform().basis.xform(dir)
	var target_dir = (target - get_global_transform().origin).normalized()
	var angle = cur_dir.angle_to(target_dir)
	var axis = cur_dir.cross(target_dir).normalized()
	 
	phys_state.set_angular_velocity(axis * (angle / phys_state.get_step()))


func _integrate_forces(phys_state):
	match self.state:
		DiscState.CONTROLLED:
			# Follow palm rotation
			if hand and hand.visible:
				look_follow(phys_state, get_global_transform().origin - \
						hand.get_palm_transform().basis.x)
		DiscState.FLYING:
			# Launch disc
			if not launched:
				print("Applying Impulse")
				phys_state.set_angular_velocity(Vector3.ZERO)
				apply_central_impulse(launch_vel)
				self.gravity_scale = 1
				launched = true
			
			# Add lift and drag forces (gravity is automatic)
			add_central_force(-self.transform.basis.z * C_LIFT * phys_state.linear_velocity.length())
			add_central_force(-C_DRAG * phys_state.linear_velocity.normalized())
