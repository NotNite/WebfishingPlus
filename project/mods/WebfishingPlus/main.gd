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
