shader_type canvas_item;
//makes noise texture
uniform sampler2D noise : repeat_enable;
uniform sampler2D mask;
uniform sampler2D fixed;
uniform sampler2D SCREEN_TEXTURE : hint_screen_texture, filter_linear_mipmap;
uniform float strength : hint_range(0.000,2) = 0.1;


void fragment() {
	float noise_value = texture(fixed,UV).r;
	float angle = noise_value * 2.0 * PI;
	vec2 direction = vec2(cos(angle), sin(angle));
	COLOR = texture(SCREEN_TEXTURE, SCREEN_UV + direction * (strength / 100.0));
	COLOR.a *= texture(mask,UV).a;
}