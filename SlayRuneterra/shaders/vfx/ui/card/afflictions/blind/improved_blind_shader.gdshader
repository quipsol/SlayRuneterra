shader_type canvas_item;
render_mode blend_mix;
uniform sampler3D noise_tex;
uniform float speed : hint_range(0.0, 0.5) = 0.5;

void vertex() { // Called for every vertex the material is visible on.
}

void fragment() { // Called for every pixel the material is visible on.
	float z_pos = fract(TIME * speed);
	vec3 pos = vec3(UV * 1.0, z_pos);
	float n = texture(noise_tex, pos).r; // noise in [0,1]
	float value = mix(.1, .35, n); // remap range for darker colors
    COLOR = vec4(vec3(value), 1.0);
}
