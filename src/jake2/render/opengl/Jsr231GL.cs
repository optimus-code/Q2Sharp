using J2N.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;

namespace Jake2.Render.Opengl
{
    public class Jsr231GL : IQGL
    {
        private GL gl;
        public Jsr231GL( )
        {
        }

        public virtual void SetGL(GL gl)
        {
            this.gl = gl;
        }

        public virtual void GlAlphaFunc(int func, float ref_renamed)
        {
            GL.AlphaFunc(func, ref_renamed);
        }

        public virtual void GlBegin(int mode)
        {
            GL.Begin(mode);
        }

        public virtual void GlBindTexture(int target, int texture)
        {
            GL.BindTexture(target, texture);
        }

        public virtual void GlBlendFunc(int sfactor, int dfactor)
        {
            GL.BlendFunc(sfactor, dfactor);
        }

        public virtual void GlClear(int mask)
        {
            GL.Clear(mask);
        }

        public virtual void GlClearColor(float red, float green, float blue, float alpha)
        {
            GL.ClearColor(red, green, blue, alpha);
        }

        public virtual void GlColor3f(float red, float green, float blue)
        {
            GL.Color3(red, green, blue);
        }

        public virtual void GlColor3ub(byte red, byte green, byte blue)
        {
            GL.Color3(red, green, blue);
        }

        public virtual void GlColor4f(float red, float green, float blue, float alpha)
        {
            GL.Color4(red, green, blue, alpha);
        }

        public virtual void GlColor4ub(byte red, byte green, byte blue, byte alpha)
        {
            GL.Color4(red, green, blue, alpha);
        }

        public virtual void GlColorPointer(int size, bool unsigned, int stride, ByteBuffer pointer)
        {
            GL.ColorPointer(size, GL_UNSIGNED_BYTE, stride, pointer);
        }

        public virtual void GlColorPointer(int size, int stride, SingleBuffer pointer)
        {
            GL.ColorPointer(size, GL_FLOAT, stride, pointer);
        }

        public virtual void GlCullFace(int mode)
        {
            gl.GlCullFace(mode);
        }

        public virtual void GlDeleteTextures(Int32Buffer textures)
        {
            GL.DeleteTextures(textures.Limit(), textures);
        }

        public virtual void GlDepthFunc(int func)
        {
            gl.GlDepthFunc(func);
        }

        public virtual void GlDepthMask(bool flag)
        {
            gl.GlDepthMask(flag);
        }

        public virtual void GlDepthRange(double zNear, double zFar)
        {
            GL.DepthRange(zNear, zFar);
        }

        public virtual void GlDisable(int cap)
        {
            gl.GlDisable(cap);
        }

        public virtual void GlDisableClientState(int cap)
        {
            gl.GlDisableClientState(cap);
        }

        public virtual void GlDrawArrays(int mode, int first, int count)
        {
            gl.GlDrawArrays(mode, first, count);
        }

        public virtual void GlDrawBuffer(int mode)
        {
            gl.GlDrawBuffer(mode);
        }

        public virtual void GlDrawElements(int mode, Int32Buffer indices)
        {
            gl.GlDrawElements(mode, indices.Limit(), GL_UNSIGNED_INT, indices);
        }

        public virtual void GlEnable(int cap)
        {
            gl.GlEnable(cap);
        }

        public virtual void GlEnableClientState(int cap)
        {
            gl.GlEnableClientState(cap);
        }

        public virtual void GlEnd()
        {
            GL.End();
        }

        public virtual void GlFinish()
        {
            GL.Finish();
        }

        public virtual void GlFlush()
        {
            gl.GlFlush();
        }

        public virtual void GlFrustum(double left, double right, double bottom, double top, double zNear, double zFar)
        {
            gl.GlFrustum(left, right, bottom, top, zNear, zFar);
        }

        public virtual int GlGetError()
        {
            return GL.GetError();
        }

        public virtual void GlGetFloat(int pname, SingleBuffer params_renamed)
        {
            gl.GlGetFloatv(pname, params_renamed);
        }

        public virtual string GlGetString(int name)
        {
            return GL.GetString(name);
        }

        public virtual void GlHint(int target, int mode)
        {
            gl.GlHint(target, mode);
        }

        public virtual void GlInterleavedArrays(int format, int stride, SingleBuffer pointer)
        {
            gl.GlInterleavedArrays(format, stride, pointer);
        }

        public virtual void GlLoadIdentity()
        {
            GL.LoadIdentity();
        }

        public virtual void GlLoadMatrix(SingleBuffer m)
        {
            GL.LoadMatrix(m);
        }

        public virtual void GlMatrixMode(int mode)
        {
            gl.GlMatrixMode(mode);
        }

        public virtual void GlOrtho(double left, double right, double bottom, double top, double zNear, double zFar)
        {
            GL.Ortho(left, right, bottom, top, zNear, zFar);
        }

        public virtual void GlPixelStorei(int pname, int param)
        {
            gl.GlPixelStorei(pname, param);
        }

        public virtual void GlPointSize(float size)
        {
            GL.PointSize(size);
        }

        public virtual void GlPolygonMode(int face, int mode)
        {
            gl.GlPolygonMode(face, mode);
        }

        public virtual void GlPopMatrix()
        {
            GL.PopMatrix();
        }

        public virtual void GlPushMatrix()
        {
            GL.PushMatrix();
        }

        public virtual void GlReadPixels(int x, int y, int width, int height, int format, int type, ByteBuffer pixels)
        {
            GL.ReadPixels(x, y, width, height, format, type, pixels);
        }

        public virtual void GlRotatef(float angle, float x, float y, float z)
        {
            GL.Rotate(angle, x, y, z);
        }

        public virtual void GlScalef(float x, float y, float z)
        {
            GL.Scale(x, y, z);
        }

        public virtual void GlScissor(int x, int y, int width, int height)
        {
            gl.GlScissor(x, y, width, height);
        }

        public virtual void GlShadeModel(int mode)
        {
            gl.GlShadeModel(mode);
        }

        public virtual void GlTexCoord2f(float s, float t)
        {
            GL.TexCoord2(s, t);
        }

        public virtual void GlTexCoordPointer(int size, int stride, SingleBuffer pointer)
        {
            gl.GlTexCoordPointer(size, GL_FLOAT, stride, pointer);
        }

        public virtual void GlTexEnvi(int target, int pname, int param)
        {
            gl.GlTexEnvi(target, pname, param);
        }

        public virtual void GlTexImage2D(int target, int level, int internalformat, int width, int height, int border, int format, int type, ByteBuffer pixels)
        {
            GL.TexImage2D(target, level, internalformat, width, height, border, format, type, pixels);
        }

        public virtual void GlTexImage2D(int target, int level, int internalformat, int width, int height, int border, int format, int type, Int32Buffer pixels)
        {
            GL.TexImage2D(target, level, internalformat, width, height, border, format, type, pixels);
        }

        public virtual void GlTexParameterf(int target, int pname, float param)
        {
            GL.TexParameter(target, pname, param);
        }

        public virtual void GlTexParameteri(int target, int pname, int param)
        {
            gl.GlTexParameteri(target, pname, param);
        }

        public virtual void GlTexSubImage2D(int target, int level, int xoffset, int yoffset, int width, int height, int format, int type, Int32Buffer pixels)
        {
            gl.GlTexSubImage2D(target, level, xoffset, yoffset, width, height, format, type, pixels);
        }

        public virtual void GlTranslatef(float x, float y, float z)
        {
            GL.Translate(x, y, z);
        }

        public virtual void GlVertex2f(float x, float y)
        {
            GL.Vertex2(x, y);
        }

        public virtual void GlVertex3f(float x, float y, float z)
        {
            GL.Vertex3(x, y, z);
        }

        public virtual void GlVertexPointer(int size, int stride, SingleBuffer pointer)
        {
            GL.VertexPointer(size, GL_FLOAT, stride, pointer);
        }

        public virtual void GlViewport(int x, int y, int width, int height)
        {
            GL.Viewport(x, y, width, height);
        }

        public virtual void GlColorTable(int target, int internalFormat, int width, int format, int type, ByteBuffer data)
        {
            gl.GlColorTable(target, internalFormat, width, format, type, data);
        }

        public virtual void GlActiveTextureARB(int texture)
        {
            gl.GlActiveTexture(texture);
        }

        public virtual void GlClientActiveTextureARB(int texture)
        {
            gl.GlClientActiveTexture(texture);
        }

        public virtual void GlPointParameterEXT(int pname, SingleBuffer pfParams)
        {
            gl.GlPointParameterfvEXT(pname, pfParams);
        }

        public virtual void GlPointParameterfEXT(int pname, float param)
        {
            gl.GlPointParameterfEXT(pname, param);
        }

        public virtual void GlLockArraysEXT(int first, int count)
        {
            gl.GlLockArraysEXT(first, count);
        }

        public virtual void GlArrayElement(int index)
        {
            GL.ArrayElement(index);
        }

        public virtual void GlUnlockArraysEXT()
        {
            gl.GlUnlockArraysEXT();
        }

        public virtual void GlMultiTexCoord2f(int target, float s, float t)
        {
            GL.MultiTexCoord2(target, s, t);
        }

        public virtual void SetSwapInterval(int interval)
        {
            gl.SetSwapInterval(interval);
        }
    }
}