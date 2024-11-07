#pragma once

#include <GLES3/gl3.h>

extern "C" {
void init();
int  draw(float x0, float y0, float z0, float w0, //
          float x1, float y1, float z1, float w1, //
          float x2, float y2, float z2, float w2, //
          float x3, float y3, float z3, float w3  //
 );
}
