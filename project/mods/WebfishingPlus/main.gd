extends Node
class_name WebfishingPlus

static func create_controller_action(map: String, code: int, av: int = 0) -> void:
	var stick_event := InputEventJoypadMotion.new()
	var button_event := InputEventJoypadButton.new()
	stick_event.device = -1
	button_event.device = -1
	
	if not InputMap.has_action(map): InputMap.add_action(map)
	
	if not av == 0:
		stick_event.axis = code
		stick_event.axis_value = av
	else:
		button_event.button_index = code
	
	InputMap.action_add_event(map, stick_event if not av == 0 else button_event)

var actor_usage_timer = {}
var packet_cache = {
	"actor_update": {},
	"actor_animation_update": {},
}

func round_vec3(vec3, precision = 1):
	return Vector3(
		round(vec3.x / precision) * precision,
		round(vec3.y / precision) * precision,
		round(vec3.z / precision) * precision
	)

func normalize_packet(orig):
	var packet = orig.duplicate(true)
	if packet.type == "actor_update":
		packet.data.pos = round_vec3(packet.data.pos, 0.5)
		packet.data.rot = round_vec3(packet.data.rot, 0.5)
		packet.data.decay_time = floor(packet.data.decay_time / 120) * 120

	if packet.type == "actor_animation_update":
		packet.data.bobber_position = round_vec3(packet.data.bobber_position)
		packet.data.land = round(packet.data.land * 5) / 5
		packet.data.player_scale_y = round(packet.data.player_scale_y * 5) / 5

	return packet

func compare(a, b):
	if a is Dictionary:
		if a.size() != b.size():
			return false
		for key in a.keys():
			if not compare(a[key], b[key]):
				return false
		return true

	if a is Array:
		if a.size() != b.size():
			return false
		for i in range(a.size()):
			if not compare(a[i], b[i]):
				return false
		return true

	if a is Vector3:
		return a.is_equal_approx(b)

	if a is float:
		return is_equal_approx(a, b)

	return a == b

func process_netcode(packet) -> bool:
	# don't memleak lol
	var now = OS.get_ticks_msec()
	for actor_id in actor_usage_timer.keys():
		if now - actor_usage_timer[actor_id] > 10000:
			actor_usage_timer.erase(actor_id)
			packet_cache["actor_update"].erase(actor_id)
			packet_cache["actor_animation_update"].erase(actor_id)

	var normalized_packet = normalize_packet(packet)
	var packet_type = normalized_packet.type
	var actor_id = normalized_packet.actor_id

	# if data is identical to last update, return true to block
	if actor_id in packet_cache[packet_type]:
		if compare(normalized_packet.data, packet_cache[packet_type][actor_id]):
			# if packet_type == "actor_update": print(normalized_packet, packet_cache[packet_type][actor_id])
			return true

	if packet_type == "actor_update" || packet_type == "actor_animation_update":
		actor_usage_timer[actor_id] = OS.get_ticks_msec()
		packet_cache[packet_type][actor_id] = normalized_packet.data

	print("WebfishingPlus: process_netcode() packet: ", packet)
	return false

func _ready() -> void:
	print("WebfishingPlus: _ready()")
	
	create_controller_action("move_left", JOY_AXIS_0, -1)
	create_controller_action("move_right", JOY_AXIS_0, 1)
	create_controller_action("move_forward", JOY_AXIS_1, -1)
	create_controller_action("move_back", JOY_AXIS_1, 1)
	create_controller_action("joy_toggle_sprint", JOY_L3)
	
	create_controller_action("joy_look_left", JOY_AXIS_2, -1)
	create_controller_action("joy_look_right", JOY_AXIS_2, 1)
	create_controller_action("joy_look_up", JOY_AXIS_3, 1)
	create_controller_action("joy_look_down", JOY_AXIS_3, -1)
	create_controller_action("joy_toggle_walk", JOY_R3)
	
	create_controller_action("bait_menu", JOY_DPAD_UP)
	create_controller_action("emote_wheel", JOY_DPAD_RIGHT)
	create_controller_action("freecam", JOY_DPAD_DOWN)
	create_controller_action("build", JOY_DPAD_LEFT)
	
	create_controller_action("bark", JOY_XBOX_Y)
	create_controller_action("kiss", JOY_XBOX_B)
	create_controller_action("move_jump", JOY_XBOX_A)
	create_controller_action("interact", JOY_XBOX_X)
	
	create_controller_action("menu_open", JOY_SELECT)
	create_controller_action("esc_menu", JOY_START)
	
	create_controller_action("joy_tab_next", JOY_R)
	create_controller_action("joy_tab_prev", JOY_L)
	
	create_controller_action("joy_zoom_control", JOY_L2)
	create_controller_action("primary_action", JOY_R2)
