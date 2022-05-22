attribute vec4 a_position;
attribute vec4 a_color;
attribute vec2 a_texCoord0;
attribute vec2 a_texCoord1;

uniform mat4 u_projTrans;

varying vec4 v_color;
varying vec2 v_texCoords;
varying vec2 v_normTexCoords;

uniform float skew;

void main() {
    v_color = a_color;
    v_texCoords = a_texCoord0;
    v_normTexCoords = a_texCoord1;
    vec4 pos = a_position;
    pos.x += skew*v_normTexCoords.y;
    gl_Position = u_projTrans * pos;
}