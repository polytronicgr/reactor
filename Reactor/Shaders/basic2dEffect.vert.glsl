﻿layout(location=0) in vec2 position;
layout(location=1) in vec2 texcoord;
layout(location=2) in vec4 color;

uniform mat4 projection;
uniform vec4 viewport;

out vec2 out_texcoords;
out vec4 out_color;

void main(){
	out_texcoords = texcoord;
	out_color = color;
	gl_Position = vec4(position, 0, 1);
}