using J2N.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Q2Sharp.Render.Opengl
{
    public class DummyGL : IQGL
    {
        private static IQGL self = new DummyGL();
        private DummyGL()
        {
        }

        public static IQGL GetInstance()
        {
            return self;
        }

        public virtual void GlAlphaFunc(int func, float ref_renamed)
        {
        }

        public virtual void GlBegin(int mode)
        {
        }

        public virtual void GlBindTexture(int target, int texture)
        {
        }

        public virtual void GlBlendFunc(int sfactor, int dfactor)
        {
        }

        public virtual void GlClear(int mask)
        {
        }

        public virtual void GlClearColor(float red, float green, float blue, float alpha)
        {
        }

        public virtual void GlColor3f(float red, float green, float blue)
        {
        }

        public virtual void GlColor3ub(byte red, byte green, byte blue)
        {
        }

        public virtual void GlColor4f(float red, float green, float blue, float alpha)
        {
        }

        public virtual void GlColor4ub(byte red, byte green, byte blue, byte alpha)
        {
        }

        public virtual void GlColorPointer(int size, bool unsigned, int stride, ByteBuffer pointer)
        {
        }

        public virtual void GlColorPointer(int size, int stride, SingleBuffer pointer)
        {
        }

        public virtual void GlCullFace(int mode)
        {
        }

        public virtual void GlDeleteTextures(Int32Buffer textures)
        {
        }

        public virtual void GlDepthFunc(int func)
        {
        }

        public virtual void GlDepthMask(bool flag)
        {
        }

        public virtual void GlDepthRange(double zNear, double zFar)
        {
        }

        public virtual void GlDisable(int cap)
        {
        }

        public virtual void GlDisableClientState(int cap)
        {
        }

        public virtual void GlDrawArrays(int mode, int first, int count)
        {
        }

        public virtual void GlDrawBuffer(int mode)
        {
        }

        public virtual void GlDrawElements(int mode, Int32Buffer indices)
        {
        }

        public virtual void GlEnable(int cap)
        {
        }

        public virtual void GlEnableClientState(int cap)
        {
        }

        public virtual void GlEnd()
        {
        }

        public virtual void GlFinish()
        {
        }

        public virtual void GlFlush()
        {
        }

        public virtual void GlFrustum(double left, double right, double bottom, double top, double zNear, double zFar)
        {
        }

        public virtual int GlGetError()
        {
            return GL_NO_ERROR;
        }

        public virtual void GlGetFloat(int pname, SingleBuffer params_renamed)
        {
        }

        public virtual string GlGetString(int name)
        {
            switch (name)

            {
                case GL_EXTENSIONS:
                    return "GL_ARB_multitexture";
                default:
                    return "";
            }
        }

        public virtual void GlHint(int target, int mode)
        {
        }

        public virtual void GlInterleavedArrays(int format, int stride, SingleBuffer pointer)
        {
        }

        public virtual void GlLoadIdentity()
        {
        }

        public virtual void GlLoadMatrix(SingleBuffer m)
        {
        }

        public virtual void GlMatrixMode(int mode)
        {
        }

        public virtual void GlOrtho(double left, double right, double bottom, double top, double zNear, double zFar)
        {
        }

        public virtual void GlPixelStorei(int pname, int param)
        {
        }

        public virtual void GlPointSize(float size)
        {
        }

        public virtual void GlPolygonMode(int face, int mode)
        {
        }

        public virtual void GlPopMatrix()
        {
        }

        public virtual void GlPushMatrix()
        {
        }

        public virtual void GlReadPixels(int x, int y, int width, int height, int format, int type, ByteBuffer pixels)
        {
        }

        public virtual void GlRotatef(float angle, float x, float y, float z)
        {
        }

        public virtual void GlScalef(float x, float y, float z)
        {
        }

        public virtual void GlScissor(int x, int y, int width, int height)
        {
        }

        public virtual void GlShadeModel(int mode)
        {
        }

        public virtual void GlTexCoord2f(float s, float t)
        {
        }

        public virtual void GlTexCoordPointer(int size, int stride, SingleBuffer pointer)
        {
        }

        public virtual void GlTexEnvi(int target, int pname, int param)
        {
        }

        public virtual void GlTexImage2D(int target, int level, int internalformat, int width, int height, int border, int format, int type, ByteBuffer pixels)
        {
        }

        public virtual void GlTexImage2D(int target, int level, int internalformat, int width, int height, int border, int format, int type, Int32Buffer pixels)
        {
        }

        public virtual void GlTexParameterf(int target, int pname, float param)
        {
        }

        public virtual void GlTexParameteri(int target, int pname, int param)
        {
        }

        public virtual void GlTexSubImage2D(int target, int level, int xoffset, int yoffset, int width, int height, int format, int type, Int32Buffer pixels)
        {
        }

        public virtual void GlTranslatef(float x, float y, float z)
        {
        }

        public virtual void GlVertex2f(float x, float y)
        {
        }

        public virtual void GlVertex3f(float x, float y, float z)
        {
        }

        public virtual void GlVertexPointer(int size, int stride, SingleBuffer pointer)
        {
        }

        public virtual void GlViewport(int x, int y, int width, int height)
        {
        }

        public virtual void GlColorTable(int target, int internalFormat, int width, int format, int type, ByteBuffer data)
        {
        }

        public virtual void GlActiveTextureARB(int texture)
        {
        }

        public virtual void GlClientActiveTextureARB(int texture)
        {
        }

        public virtual void GlPointParameterEXT(int pname, SingleBuffer pfParams)
        {
        }

        public virtual void GlPointParameterfEXT(int pname, float param)
        {
        }

        public virtual void GlLockArraysEXT(int first, int count)
        {
        }

        public virtual void GlArrayElement(int index)
        {
        }

        public virtual void GlUnlockArraysEXT()
        {
        }

        public virtual void GlMultiTexCoord2f(int target, float s, float t)
        {
        }

        public virtual void SetSwapInterval(int interval)
        {
        }
    }
}