using J2N.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jake2.Render.Opengl
{
    public class CountGL : IQGL
    {
        private static int count = 0;
        private static IQGL self = new CountGL();
        private CountGL()
        {
        }

        public static IQGL GetInstance()
        {
            return self;
        }

        public virtual void GlAlphaFunc(int func, float ref_renamed)
        {
            ++count;
        }

        public virtual void GlBindTexture(int target, int texture)
        {
            ++count;
        }

        public virtual void GlBlendFunc(int sfactor, int dfactor)
        {
            ++count;
        }

        public virtual void GlClear(int mask)
        {
            ++count;
        }

        public virtual void GlClearColor(float red, float green, float blue, float alpha)
        {
            ++count;
        }

        public virtual void GlColor3f(float red, float green, float blue)
        {
            ++count;
        }

        public virtual void GlColor3ub(byte red, byte green, byte blue)
        {
            ++count;
        }

        public virtual void GlColor4f(float red, float green, float blue, float alpha)
        {
            ++count;
        }

        public virtual void GlColor4ub(byte red, byte green, byte blue, byte alpha)
        {
            ++count;
        }

        public virtual void GlColorPointer(int size, bool unsigned, int stride, ByteBuffer pointer)
        {
            ++count;
        }

        public virtual void GlColorPointer(int size, int stride, SingleBuffer pointer)
        {
            ++count;
        }

        public virtual void GlCullFace(int mode)
        {
            ++count;
        }

        public virtual void GlDeleteTextures(Int32Buffer textures)
        {
            ++count;
        }

        public virtual void GlDepthFunc(int func)
        {
            ++count;
        }

        public virtual void GlDepthMask(bool flag)
        {
            ++count;
        }

        public virtual void GlDepthRange(double zNear, double zFar)
        {
            ++count;
        }

        public virtual void GlDisable(int cap)
        {
            ++count;
        }

        public virtual void GlDisableClientState(int cap)
        {
            ++count;
        }

        public virtual void GlDrawArrays(int mode, int first, int count)
        {
            ++count;
        }

        public virtual void GlDrawBuffer(int mode)
        {
            ++count;
        }

        public virtual void GlDrawElements(int mode, Int32Buffer indices)
        {
            ++count;
        }

        public virtual void GlEnable(int cap)
        {
            ++count;
        }

        public virtual void GlEnableClientState(int cap)
        {
            ++count;
        }

        public virtual void GlEnd()
        {
            ++count;
        }

        public virtual void GlFinish()
        {
            ++count;
        }

        public virtual void GlFlush()
        {
            System.Diagnostics.Debug.WriteLine("GL calls/frame: " + (++count));
            count = 0;
        }

        public virtual void GlFrustum(double left, double right, double bottom, double top, double zNear, double zFar)
        {
            ++count;
        }

        public virtual int GlGetError()
        {
            return GL_NO_ERROR;
        }

        public virtual void GlGetFloat(int pname, SingleBuffer params_renamed)
        {
            ++count;
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
            ++count;
        }

        public virtual void GlInterleavedArrays(int format, int stride, SingleBuffer pointer)
        {
            ++count;
        }

        public virtual void GlLoadIdentity()
        {
            ++count;
        }

        public virtual void GlLoadMatrix(SingleBuffer m)
        {
            ++count;
        }

        public virtual void GlMatrixMode(int mode)
        {
            ++count;
        }

        public virtual void GlOrtho(double left, double right, double bottom, double top, double zNear, double zFar)
        {
            ++count;
        }

        public virtual void GlPixelStorei(int pname, int param)
        {
            ++count;
        }

        public virtual void GlPointSize(float size)
        {
            ++count;
        }

        public virtual void GlPolygonMode(int face, int mode)
        {
            ++count;
        }

        public virtual void GlPopMatrix()
        {
            ++count;
        }

        public virtual void GlPushMatrix()
        {
            ++count;
        }

        public virtual void GlReadPixels(int x, int y, int width, int height, int format, int type, ByteBuffer pixels)
        {
            ++count;
        }

        public virtual void GlRotatef(float angle, float x, float y, float z)
        {
            ++count;
        }

        public virtual void GlScalef(float x, float y, float z)
        {
            ++count;
        }

        public virtual void GlScissor(int x, int y, int width, int height)
        {
            ++count;
        }

        public virtual void GlShadeModel(int mode)
        {
            ++count;
        }

        public virtual void GlTexCoord2f(float s, float t)
        {
            ++count;
        }

        public virtual void GlTexCoordPointer(int size, int stride, SingleBuffer pointer)
        {
            ++count;
        }

        public virtual void GlTexEnvi(int target, int pname, int param)
        {
            ++count;
        }

        public virtual void GlTexImage2D(int target, int level, int internalformat, int width, int height, int border, int format, int type, ByteBuffer pixels)
        {
            ++count;
        }

        public virtual void GlTexImage2D(int target, int level, int internalformat, int width, int height, int border, int format, int type, Int32Buffer pixels)
        {
            ++count;
        }

        public virtual void GlTexParameterf(int target, int pname, float param)
        {
            ++count;
        }

        public virtual void GlTexParameteri(int target, int pname, int param)
        {
            ++count;
        }

        public virtual void GlTexSubImage2D(int target, int level, int xoffset, int yoffset, int width, int height, int format, int type, Int32Buffer pixels)
        {
            ++count;
        }

        public virtual void GlTranslatef(float x, float y, float z)
        {
            ++count;
        }

        public virtual void GlVertex2f(float x, float y)
        {
            ++count;
        }

        public virtual void GlVertex3f(float x, float y, float z)
        {
            ++count;
        }

        public virtual void GlVertexPointer(int size, int stride, SingleBuffer pointer)
        {
            ++count;
        }

        public virtual void GlViewport(int x, int y, int width, int height)
        {
            ++count;
        }

        public virtual void GlColorTable(int target, int internalFormat, int width, int format, int type, ByteBuffer data)
        {
            ++count;
        }

        public virtual void GlActiveTextureARB(int texture)
        {
            ++count;
        }

        public virtual void GlClientActiveTextureARB(int texture)
        {
            ++count;
        }

        public virtual void GlPointParameterEXT(int pname, SingleBuffer pfParams)
        {
            ++count;
        }

        public virtual void GlPointParameterfEXT(int pname, float param)
        {
            ++count;
        }

        public virtual void GlLockArraysEXT(int first, int count)
        {
            ++count;
        }

        public virtual void GlArrayElement(int index)
        {
            ++count;
        }

        public virtual void GlUnlockArraysEXT()
        {
            ++count;
        }

        public virtual void GlMultiTexCoord2f(int target, float s, float t)
        {
            ++count;
        }

        public virtual void SetSwapInterval(int interval)
        {
            ++count;
        }
    }
}