shader_type canvas_item;
render_mode blend_mix;
uniform sampler2D noise_tex;
uniform vec2 scroll_speed = vec2(0.0, 0.001);

void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	// Called for every pixel the material is visible on.
	vec2 uv = fract(UV + TIME * scroll_speed);

	float n = texture(noise_tex, uv).r; // noise in [0,1]

    // Map 0..1 → dark gray (0.2) to black (0.0)
	float value = mix(0.35, 0.1, n);

    COLOR = vec4(vec3(value), 1.0);
}

//void light() {
//	// Called for every pixel for every light affecting the material.
//	// Uncomment to replace the default light processing function with this one.
//}
