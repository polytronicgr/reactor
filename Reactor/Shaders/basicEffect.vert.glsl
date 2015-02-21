﻿#include "headers.glsl"

uniform mat4 world : WORLD;
uniform mat4 view : VIEW;
uniform mat4 projection : PROJECTION;
out vec3 out_normal;
out vec2 out_texcoord;
    void main() {
        vec4 vPosition = vec4(r_Position, 1.0f);
        out_normal = vec4(r_Normal, 1.0f).xyz;
        out_texcoord = r_TexCoord;
		gl_Position = projection * view * world * vPosition;
	}
