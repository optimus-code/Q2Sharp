using J2N.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Q2Sharp.Render.Opengl
{
    public class LwjglGL : IQGL
    {
        public LwjglGL( )
        {
        }

        public void GlAlphaFunc(int func, float ref_renamed)
        {
            GL11.GlAlphaFunc(func, ref_renamed);
        }

        public void GlBegin(int mode)
        {
            GL11.GlBegin(mode);
        }

        public void GlBindTexture(int target, int texture)
        {
            GL11.GlBindTexture(target, texture);
        }

        public void GlBlendFunc(int sfactor, int dfactor)
        {
            GL11.GlBlendFunc(sfactor, dfactor);
        }

        public void GlClear(int mask)
        {
            GL11.GlClear(mask);
        }

        public void GlClearColor(float red, float green, float blue, float alpha)
        {
            GL11.GlClearColor(red, green, blue, alpha);
        }

        public void GlColor3f(float red, float green, float blue)
        {
            GL11.GlColor3f(red, green, blue);
        }

        public void GlColor3ub(byte red, byte green, byte blue)
        {
            GL11.GlColor3ub(red, green, blue);
        }

        public void GlColor4f(float red, float green, float blue, float alpha)
        {
            GL11.GlColor4f(red, green, blue, alpha);
        }

        public void GlColor4ub(byte red, byte green, byte blue, byte alpha)
        {
            GL11.GlColor4ub(red, green, blue, alpha);
        }

        public void GlColorPointer(int size, bool unsigned, int stride, ByteBuffer pointer)
        {
            GL11.GlColorPointer(size, unsigned, stride, pointer);
        }

        public void GlColorPointer(int size, int stride, SingleBuffer pointer)
        {
            GL11.GlColorPointer(size, stride, pointer);
        }

        public void GlCullFace(int mode)
        {
            GL11.GlCullFace(mode);
        }

        public void GlDeleteTextures(Int32Buffer textures)
        {
            GL11.GlDeleteTextures(textures);
        }

        public void GlDepthFunc(int func)
        {
            GL11.GlDepthFunc(func);
        }

        public void GlDepthMask(bool flag)
        {
            GL11.GlDepthMask(flag);
        }

        public void GlDepthRange(double zNear, double zFar)
        {
            GL11.GlDepthRange(zNear, zFar);
        }

        public void GlDisable(int cap)
        {
            GL11.GlDisable(cap);
        }

        public void GlDisableClientState(int cap)
        {
            GL11.GlDisableClientState(cap);
        }

        public void GlDrawArrays(int mode, int first, int count)
        {
            GL11.GlDrawArrays(mode, first, count);
        }

        public void GlDrawBuffer(int mode)
        {
            GL11.GlDrawBuffer(mode);
        }

        public void GlDrawElements(int mode, Int32Buffer indices)
        {
            GL11.GlDrawElements(mode, indices);
        }

        public void GlEnable(int cap)
        {
            GL11.GlEnable(cap);
        }

        public void GlEnableClientState(int cap)
        {
            GL11.GlEnableClientState(cap);
        }

        public void GlEnd()
        {
            GL11.GlEnd();
        }

        public void GlFinish()
        {
            GL11.GlFinish();
        }

        public void GlFlush()
        {
            GL11.GlFlush();
        }

        public void GlFrustum(double left, double right, double bottom, double top, double zNear, double zFar)
        {
            GL11.GlFrustum(left, right, bottom, top, zNear, zFar);
        }

        public int GlGetError()
        {
            return GL11.GlGetError();
        }

        public void GlGetFloat(int pname, SingleBuffer params_renamed)
        {
            GL11.GlGetFloat(pname, params_renamed);
        }

        public string GlGetString(int name)
        {
            return GL11.GlGetString(name);
        }

        public virtual void GlHint(int target, int mode)
        {
            GL11.GlHint(target, mode);
        }

        public void GlInterleavedArrays(int format, int stride, SingleBuffer pointer)
        {
            GL11.GlInterleavedArrays(format, stride, pointer);
        }

        public void GlLoadIdentity()
        {
            GL11.GlLoadIdentity();
        }

        public void GlLoadMatrix(SingleBuffer m)
        {
            GL11.GlLoadMatrix(m);
        }

        public void GlMatrixMode(int mode)
        {
            GL11.GlMatrixMode(mode);
        }

        public void GlOrtho(double left, double right, double bottom, double top, double zNear, double zFar)
        {
            GL11.GlOrtho(left, right, bottom, top, zNear, zFar);
        }

        public void GlPixelStorei(int pname, int param)
        {
            GL11.GlPixelStorei(pname, param);
        }

        public void GlPointSize(float size)
        {
            GL11.GlPointSize(size);
        }

        public void GlPolygonMode(int face, int mode)
        {
            GL11.GlPolygonMode(face, mode);
        }

        public void GlPopMatrix()
        {
            GL11.GlPopMatrix();
        }

        public void GlPushMatrix()
        {
            GL11.GlPushMatrix();
        }

        public void GlReadPixels(int x, int y, int width, int height, int format, int type, ByteBuffer pixels)
        {
            GL11.GlReadPixels(x, y, width, height, format, type, pixels);
        }

        public void GlRotatef(float angle, float x, float y, float z)
        {
            GL11.GlRotatef(angle, x, y, z);
        }

        public void GlScalef(float x, float y, float z)
        {
            GL11.GlScalef(x, y, z);
        }

        public void GlScissor(int x, int y, int width, int height)
        {
            GL11.GlScissor(x, y, width, height);
        }

        public void GlShadeModel(int mode)
        {
            GL11.GlShadeModel(mode);
        }

        public void GlTexCoord2f(float s, float t)
        {
            GL11.GlTexCoord2f(s, t);
        }

        public void GlTexCoordPointer(int size, int stride, SingleBuffer pointer)
        {
            GL11.GlTexCoordPointer(size, stride, pointer);
        }

        public void GlTexEnvi(int target, int pname, int param)
        {
            GL11.GlTexEnvi(target, pname, param);
        }

        public void GlTexImage2D(int target, int level, int internalformat, int width, int height, int border, int format, int type, ByteBuffer pixels)
        {
            GL11.GlTexImage2D(target, level, internalformat, width, height, border, format, type, pixels);
        }

        public void GlTexImage2D(int target, int level, int internalformat, int width, int height, int border, int format, int type, Int32Buffer pixels)
        {
            GL11.GlTexImage2D(target, level, internalformat, width, height, border, format, type, pixels);
        }

        public void GlTexParameterf(int target, int pname, float param)
        {
            GL11.GlTexParameterf(target, pname, param);
        }

        public void GlTexParameteri(int target, int pname, int param)
        {
            GL11.GlTexParameteri(target, pname, param);
        }

        public void GlTexSubImage2D(int target, int level, int xoffset, int yoffset, int width, int height, int format, int type, Int32Buffer pixels)
        {
            GL11.GlTexSubImage2D(target, level, xoffset, yoffset, width, height, format, type, pixels);
        }

        public void GlTranslatef(float x, float y, float z)
        {
            GL11.GlTranslatef(x, y, z);
        }

        public void GlVertex2f(float x, float y)
        {
            GL11.GlVertex2f(x, y);
        }

        public void GlVertex3f(float x, float y, float z)
        {
            GL11.GlVertex3f(x, y, z);
        }

        public void GlVertexPointer(int size, int stride, SingleBuffer pointer)
        {
            GL11.GlVertexPointer(size, stride, pointer);
        }

        public void GlViewport(int x, int y, int width, int height)
        {
            GL11.GlViewport(x, y, width, height);
        }

        public void GlColorTable(int target, int internalFormat, int width, int format, int type, ByteBuffer data)
        {
            EXTPalettedTexture.GlColorTableEXT(target, internalFormat, width, format, type, data);
        }

        public void GlActiveTextureARB(int texture)
        {
            ARBMultitexture.GlActiveTextureARB(texture);
        }

        public void GlClientActiveTextureARB(int texture)
        {
            ARBMultitexture.GlClientActiveTextureARB(texture);
        }

        public void GlPointParameterEXT(int pname, SingleBuffer pfParams)
        {
            EXTPointParameters.GlPointParameterEXT(pname, pfParams);
        }

        public void GlPointParameterfEXT(int pname, float param)
        {
            EXTPointParameters.GlPointParameterfEXT(pname, param);
        }

        public void GlLockArraysEXT(int first, int count)
        {
            EXTCompiledVertexArray.GlLockArraysEXT(first, count);
        }

        public void GlArrayElement(int index)
        {
            GL11.GlArrayElement(index);
        }

        public void GlUnlockArraysEXT()
        {
            EXTCompiledVertexArray.GlUnlockArraysEXT();
        }

        public void GlMultiTexCoord2f(int target, float s, float t)
        {
            GL13.GlMultiTexCoord2f(target, s, t);
        }

        public virtual void SetSwapInterval(int interval)
        {
            Display.SetSwapInterval(interval);
        }
    }
}