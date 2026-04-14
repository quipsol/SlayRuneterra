shader_type canvas_item;
//makes noise texture
uniform sampler2D noise : repeat_enable;
uniform sampler2D mask;
uniform sampler2D SCREEN_TEXTURE : hint_screen_texture, filter_linear_mipmap;
uniform float speed : hint_range(0.002,0.5) = 0.1;
uniform float strength : hint_range(0.002,0.5) = 0.1;

uniform vec2 direction = vec2(0.3, 0.3);

void fragment() {
	//sample noise texture
	vec2 normalizedDir = normalize(direction);
	float noise_value = texture(noise,UV+-TIME * speed * normalizedDir).r;
	COLOR = texture(SCREEN_TEXTURE,SCREEN_UV - (strength/2.0) + vec2(noise_value) * strength);

	COLOR.a *= texture(mask,UV).a;

}