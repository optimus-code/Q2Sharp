/*
 * Anorms.java
 * Copyright (C) 2003
 *
 * $Id: Anorms.java,v 1.1 2004-07-09 06:50:49 hzi Exp $
 */
/*
Copyright (C) 1997-2001 Id Software, Inc.

This program is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.

See the GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.

*/
package jake2.render.fastjogl;

/**
 * Anorms
 *  
 * @author cwei
 */
public interface Anorms {
	
	final float[][] VERTEXNORMALS = {
		{-0.525731f, 0.000000f, 0.850651f}, 
		{-0.442863f, 0.238856f, 0.864188f}, 
		{-0.295242f, 0.000000f, 0.955423f}, 
		{-0.309017f, 0.500000f, 0.809017f}, 
		{-0.162460f, 0.262866f, 0.951056f}, 
		{0.000000f, 0.000000f, 1.000000f}, 
		{0.000000f, 0.850651f, 0.525731f}, 
		{-0.147621f, 0.716567f, 0.681718f}, 
		{0.147621f, 0.716567f, 0.681718f}, 
		{0.000000f, 0.525731f, 0.850651f}, 
		{0.309017f, 0.500000f, 0.809017f}, 
		{0.525731f, 0.000000f, 0.850651f}, 
		{0.295242f, 0.000000f, 0.955423f}, 
		{0.442863f, 0.238856f, 0.864188f}, 
		{0.162460f, 0.262866f, 0.951056f}, 
		{-0.681718f, 0.147621f, 0.716567f}, 
		{-0.809017f, 0.309017f, 0.500000f}, 
		{-0.587785f, 0.425325f, 0.688191f}, 
		{-0.850651f, 0.525731f, 0.000000f}, 
		{-0.864188f, 0.442863f, 0.238856f}, 
		{-0.716567f, 0.681718f, 0.147621f}, 
		{-0.688191f, 0.587785f, 0.425325f}, 
		{-0.500000f, 0.809017f, 0.309017f}, 
		{-0.238856f, 0.864188f, 0.442863f}, 
		{-0.425325f, 0.688191f, 0.587785f}, 
		{-0.716567f, 0.681718f, -0.147621f}, 
		{-0.500000f, 0.809017f, -0.309017f}, 
		{-0.525731f, 0.850651f, 0.000000f}, 
		{0.000000f, 0.850651f, -0.525731f}, 
		{-0.238856f, 0.864188f, -0.442863f}, 
		{0.000000f, 0.955423f, -0.295242f}, 
		{-0.262866f, 0.951056f, -0.162460f}, 
		{0.000000f, 1.000000f, 0.000000f}, 
		{0.000000f, 0.955423f, 0.295242f}, 
		{-0.262866f, 0.951056f, 0.162460f}, 
		{0.238856f, 0.864188f, 0.442863f}, 
		{0.262866f, 0.951056f, 0.162460f}, 
		{0.500000f, 0.809017f, 0.309017f}, 
		{0.238856f, 0.864188f, -0.442863f}, 
		{0.262866f, 0.951056f, -0.162460f}, 
		{0.500000f, 0.809017f, -0.309017f}, 
		{0.850651f, 0.525731f, 0.000000f}, 
		{0.716567f, 0.681718f, 0.147621f}, 
		{0.716567f, 0.681718f, -0.147621f}, 
		{0.525731f, 0.850651f, 0.000000f}, 
		{0.425325f, 0.688191f, 0.587785f}, 
		{0.864188f, 0.442863f, 0.238856f}, 
		{0.688191f, 0.587785f, 0.425325f}, 
		{0.809017f, 0.309017f, 0.500000f}, 
		{0.681718f, 0.147621f, 0.716567f}, 
		{0.587785f, 0.425325f, 0.688191f}, 
		{0.955423f, 0.295242f, 0.000000f}, 
		{1.000000f, 0.000000f, 0.000000f}, 
		{0.951056f, 0.162460f, 0.262866f}, 
		{0.850651f, -0.525731f, 0.000000f}, 
		{0.955423f, -0.295242f, 0.000000f}, 
		{0.864188f, -0.442863f, 0.238856f}, 
		{0.951056f, -0.162460f, 0.262866f}, 
		{0.809017f, -0.309017f, 0.500000f}, 
		{0.681718f, -0.147621f, 0.716567f}, 
		{0.850651f, 0.000000f, 0.525731f}, 
		{0.864188f, 0.442863f, -0.238856f}, 
		{0.809017f, 0.309017f, -0.500000f}, 
		{0.951056f, 0.162460f, -0.262866f}, 
		{0.525731f, 0.000000f, -0.850651f}, 
		{0.681718f, 0.147621f, -0.716567f}, 
		{0.681718f, -0.147621f, -0.716567f}, 
		{0.850651f, 0.000000f, -0.525731f}, 
		{0.809017f, -0.309017f, -0.500000f}, 
		{0.864188f, -0.442863f, -0.238856f}, 
		{0.951056f, -0.162460f, -0.262866f}, 
		{0.147621f, 0.716567f, -0.681718f}, 
		{0.309017f, 0.500000f, -0.809017f}, 
		{0.425325f, 0.688191f, -0.587785f}, 
		{0.442863f, 0.238856f, -0.864188f}, 
		{0.587785f, 0.425325f, -0.688191f}, 
		{0.688191f, 0.587785f, -0.425325f}, 
		{-0.147621f, 0.716567f, -0.681718f}, 
		{-0.309017f, 0.500000f, -0.809017f}, 
		{0.000000f, 0.525731f, -0.850651f}, 
		{-0.525731f, 0.000000f, -0.850651f}, 
		{-0.442863f, 0.238856f, -0.864188f}, 
		{-0.295242f, 0.000000f, -0.955423f}, 
		{-0.162460f, 0.262866f, -0.951056f}, 
		{0.000000f, 0.000000f, -1.000000f}, 
		{0.295242f, 0.000000f, -0.955423f}, 
		{0.162460f, 0.262866f, -0.951056f}, 
		{-0.442863f, -0.238856f, -0.864188f}, 
		{-0.309017f, -0.500000f, -0.809017f}, 
		{-0.162460f, -0.262866f, -0.951056f}, 
		{0.000000f, -0.850651f, -0.525731f}, 
		{-0.147621f, -0.716567f, -0.681718f}, 
		{0.147621f, -0.716567f, -0.681718f}, 
		{0.000000f, -0.525731f, -0.850651f}, 
		{0.309017f, -0.500000f, -0.809017f}, 
		{0.442863f, -0.238856f, -0.864188f}, 
		{0.162460f, -0.262866f, -0.951056f}, 
		{0.238856f, -0.864188f, -0.442863f}, 
		{0.500000f, -0.809017f, -0.309017f}, 
		{0.425325f, -0.688191f, -0.587785f}, 
		{0.716567f, -0.681718f, -0.147621f}, 
		{0.688191f, -0.587785f, -0.425325f}, 
		{0.587785f, -0.425325f, -0.688191f}, 
		{0.000000f, -0.955423f, -0.295242f}, 
		{0.000000f, -1.000000f, 0.000000f}, 
		{0.262866f, -0.951056f, -0.162460f}, 
		{0.000000f, -0.850651f, 0.525731f}, 
		{0.000000f, -0.955423f, 0.295242f}, 
		{0.238856f, -0.864188f, 0.442863f}, 
		{0.262866f, -0.951056f, 0.162460f}, 
		{0.500000f, -0.809017f, 0.309017f}, 
		{0.716567f, -0.681718f, 0.147621f}, 
		{0.525731f, -0.850651f, 0.000000f}, 
		{-0.238856f, -0.864188f, -0.442863f}, 
		{-0.500000f, -0.809017f, -0.309017f}, 
		{-0.262866f, -0.951056f, -0.162460f}, 
		{-0.850651f, -0.525731f, 0.000000f}, 
		{-0.716567f, -0.681718f, -0.147621f}, 
		{-0.716567f, -0.681718f, 0.147621f}, 
		{-0.525731f, -0.850651f, 0.000000f}, 
		{-0.500000f, -0.809017f, 0.309017f}, 
		{-0.238856f, -0.864188f, 0.442863f}, 
		{-0.262866f, -0.951056f, 0.162460f}, 
		{-0.864188f, -0.442863f, 0.238856f}, 
		{-0.809017f, -0.309017f, 0.500000f}, 
		{-0.688191f, -0.587785f, 0.425325f}, 
		{-0.681718f, -0.147621f, 0.716567f}, 
		{-0.442863f, -0.238856f, 0.864188f}, 
		{-0.587785f, -0.425325f, 0.688191f}, 
		{-0.309017f, -0.500000f, 0.809017f}, 
		{-0.147621f, -0.716567f, 0.681718f}, 
		{-0.425325f, -0.688191f, 0.587785f}, 
		{-0.162460f, -0.262866f, 0.951056f}, 
		{0.442863f, -0.238856f, 0.864188f}, 
		{0.162460f, -0.262866f, 0.951056f}, 
		{0.309017f, -0.500000f, 0.809017f}, 
		{0.147621f, -0.716567f, 0.681718f}, 
		{0.000000f, -0.525731f, 0.850651f}, 
		{0.425325f, -0.688191f, 0.587785f}, 
		{0.587785f, -0.425325f, 0.688191f}, 
		{0.688191f, -0.587785f, 0.425325f}, 
		{-0.955423f, 0.295242f, 0.000000f}, 
		{-0.951056f, 0.162460f, 0.262866f}, 
		{-1.000000f, 0.000000f, 0.000000f}, 
		{-0.850651f, 0.000000f, 0.525731f}, 
		{-0.955423f, -0.295242f, 0.000000f}, 
		{-0.951056f, -0.162460f, 0.262866f}, 
		{-0.864188f, 0.442863f, -0.238856f}, 
		{-0.951056f, 0.162460f, -0.262866f}, 
		{-0.809017f, 0.309017f, -0.500000f}, 
		{-0.864188f, -0.442863f, -0.238856f}, 
		{-0.951056f, -0.162460f, -0.262866f}, 
		{-0.809017f, -0.309017f, -0.500000f}, 
		{-0.681718f, 0.147621f, -0.716567f}, 
		{-0.681718f, -0.147621f, -0.716567f}, 
		{-0.850651f, 0.000000f, -0.525731f}, 
		{-0.688191f, 0.587785f, -0.425325f}, 
		{-0.587785f, 0.425325f, -0.688191f}, 
		{-0.425325f, 0.688191f, -0.587785f}, 
		{-0.425325f, -0.688191f, -0.587785f}, 
		{-0.587785f, -0.425325f, -0.688191f}, 
		{-0.688191f, -0.587785f, -0.425325f}
	};
	
	final float[][] VERTEXNORMAL_DOTS = {
		{1.23f,1.30f,1.47f,1.35f,1.56f,1.71f,1.37f,1.38f,1.59f,1.60f,1.79f,1.97f,1.88f,1.92f,1.79f,1.02f,0.93f,1.07f,0.82f,0.87f,0.88f,0.94f,0.96f,1.14f,1.11f,0.82f,0.83f,0.89f,0.89f,0.86f,0.94f,0.91f,1.00f,1.21f,0.98f,1.48f,1.30f,1.57f,0.96f,1.07f,1.14f,1.60f,1.61f,1.40f,1.37f,1.72f,1.78f,1.79f,1.93f,1.99f,1.90f,1.68f,1.71f,1.86f,1.60f,1.68f,1.78f,1.86f,1.93f,1.99f,1.97f,1.44f,1.22f,1.49f,0.93f,0.99f,0.99f,1.23f,1.22f,1.44f,1.49f,0.89f,0.89f,0.97f,0.91f,0.98f,1.19f,0.82f,0.76f,0.82f,0.71f,0.72f,0.73f,0.76f,0.79f,0.86f,0.83f,0.72f,0.76f,0.76f,0.89f,0.82f,0.89f,0.82f,0.89f,0.91f,0.83f,0.96f,1.14f,0.97f,1.40f,1.19f,0.98f,0.94f,1.00f,1.07f,1.37f,1.21f,1.48f,1.30f,1.57f,1.61f,1.37f,0.86f,0.83f,0.91f,0.82f,0.82f,0.88f,0.89f,0.96f,1.14f,0.98f,0.87f,0.93f,0.94f,1.02f,1.30f,1.07f,1.35f,1.38f,1.11f,1.56f,1.92f,1.79f,1.79f,1.59f,1.60f,1.72f,1.90f,1.79f,0.80f,0.85f,0.79f,0.93f,0.80f,0.85f,0.77f,0.74f,0.72f,0.77f,0.74f,0.72f,0.70f,0.70f,0.71f,0.76f,0.73f,0.79f,0.79f,0.73f,0.76f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f},
		{1.26f,1.26f,1.48f,1.23f,1.50f,1.71f,1.14f,1.19f,1.38f,1.46f,1.64f,1.94f,1.87f,1.84f,1.71f,1.02f,0.92f,1.00f,0.79f,0.85f,0.84f,0.91f,0.90f,0.98f,0.99f,0.77f,0.77f,0.83f,0.82f,0.79f,0.86f,0.84f,0.92f,0.99f,0.91f,1.24f,1.03f,1.33f,0.88f,0.94f,0.97f,1.41f,1.39f,1.18f,1.11f,1.51f,1.61f,1.59f,1.80f,1.91f,1.76f,1.54f,1.65f,1.76f,1.70f,1.70f,1.85f,1.85f,1.97f,1.99f,1.93f,1.28f,1.09f,1.39f,0.92f,0.97f,0.99f,1.18f,1.26f,1.52f,1.48f,0.83f,0.85f,0.90f,0.88f,0.93f,1.00f,0.77f,0.73f,0.78f,0.72f,0.71f,0.74f,0.75f,0.79f,0.86f,0.81f,0.75f,0.81f,0.79f,0.96f,0.88f,0.94f,0.86f,0.93f,0.92f,0.85f,1.08f,1.33f,1.05f,1.55f,1.31f,1.01f,1.05f,1.27f,1.31f,1.60f,1.47f,1.70f,1.54f,1.76f,1.76f,1.57f,0.93f,0.90f,0.99f,0.88f,0.88f,0.95f,0.97f,1.11f,1.39f,1.20f,0.92f,0.97f,1.01f,1.10f,1.39f,1.22f,1.51f,1.58f,1.32f,1.64f,1.97f,1.85f,1.91f,1.77f,1.74f,1.88f,1.99f,1.91f,0.79f,0.86f,0.80f,0.94f,0.84f,0.88f,0.74f,0.74f,0.71f,0.82f,0.77f,0.76f,0.70f,0.73f,0.72f,0.73f,0.70f,0.74f,0.85f,0.77f,0.82f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f},
		{1.34f,1.27f,1.53f,1.17f,1.46f,1.71f,0.98f,1.05f,1.20f,1.34f,1.48f,1.86f,1.82f,1.71f,1.62f,1.09f,0.94f,0.99f,0.79f,0.85f,0.82f,0.90f,0.87f,0.93f,0.96f,0.76f,0.74f,0.79f,0.76f,0.74f,0.79f,0.78f,0.85f,0.92f,0.85f,1.00f,0.93f,1.06f,0.81f,0.86f,0.89f,1.16f,1.12f,0.97f,0.95f,1.28f,1.38f,1.35f,1.60f,1.77f,1.57f,1.33f,1.50f,1.58f,1.69f,1.63f,1.82f,1.74f,1.91f,1.92f,1.80f,1.04f,0.97f,1.21f,0.90f,0.93f,0.97f,1.05f,1.21f,1.48f,1.37f,0.77f,0.80f,0.84f,0.85f,0.88f,0.92f,0.73f,0.71f,0.74f,0.74f,0.71f,0.75f,0.73f,0.79f,0.84f,0.78f,0.79f,0.86f,0.81f,1.05f,0.94f,0.99f,0.90f,0.95f,0.92f,0.86f,1.24f,1.44f,1.14f,1.59f,1.34f,1.02f,1.27f,1.50f,1.49f,1.80f,1.69f,1.86f,1.72f,1.87f,1.80f,1.69f,1.00f,0.98f,1.23f,0.95f,0.96f,1.09f,1.16f,1.37f,1.63f,1.46f,0.99f,1.10f,1.25f,1.24f,1.51f,1.41f,1.67f,1.77f,1.55f,1.72f,1.95f,1.89f,1.98f,1.91f,1.86f,1.97f,1.99f,1.94f,0.81f,0.89f,0.85f,0.98f,0.90f,0.94f,0.75f,0.78f,0.73f,0.89f,0.83f,0.82f,0.72f,0.77f,0.76f,0.72f,0.70f,0.71f,0.91f,0.83f,0.89f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f},
		{1.46f,1.34f,1.60f,1.16f,1.46f,1.71f,0.94f,0.99f,1.05f,1.26f,1.33f,1.74f,1.76f,1.57f,1.54f,1.23f,0.98f,1.05f,0.83f,0.89f,0.84f,0.92f,0.87f,0.91f,0.96f,0.78f,0.74f,0.79f,0.72f,0.72f,0.75f,0.76f,0.80f,0.88f,0.83f,0.94f,0.87f,0.95f,0.76f,0.80f,0.82f,0.97f,0.96f,0.89f,0.88f,1.08f,1.11f,1.10f,1.37f,1.59f,1.37f,1.07f,1.27f,1.34f,1.57f,1.45f,1.69f,1.55f,1.77f,1.79f,1.60f,0.93f,0.90f,0.99f,0.86f,0.87f,0.93f,0.96f,1.07f,1.35f,1.18f,0.73f,0.76f,0.77f,0.81f,0.82f,0.85f,0.70f,0.71f,0.72f,0.78f,0.73f,0.77f,0.73f,0.79f,0.82f,0.76f,0.83f,0.90f,0.84f,1.18f,0.98f,1.03f,0.92f,0.95f,0.90f,0.86f,1.32f,1.45f,1.15f,1.53f,1.27f,0.99f,1.42f,1.65f,1.58f,1.93f,1.83f,1.94f,1.81f,1.88f,1.74f,1.70f,1.19f,1.17f,1.44f,1.11f,1.15f,1.36f,1.41f,1.61f,1.81f,1.67f,1.22f,1.34f,1.50f,1.42f,1.65f,1.61f,1.82f,1.91f,1.75f,1.80f,1.89f,1.89f,1.98f,1.99f,1.94f,1.98f,1.92f,1.87f,0.86f,0.95f,0.92f,1.14f,0.98f,1.03f,0.79f,0.84f,0.77f,0.97f,0.90f,0.89f,0.76f,0.82f,0.82f,0.74f,0.72f,0.71f,0.98f,0.89f,0.97f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f},
		{1.60f,1.44f,1.68f,1.22f,1.49f,1.71f,0.93f,0.99f,0.99f,1.23f,1.22f,1.60f,1.68f,1.44f,1.49f,1.40f,1.14f,1.19f,0.89f,0.96f,0.89f,0.97f,0.89f,0.91f,0.98f,0.82f,0.76f,0.82f,0.71f,0.72f,0.73f,0.76f,0.79f,0.86f,0.83f,0.91f,0.83f,0.89f,0.72f,0.76f,0.76f,0.89f,0.89f,0.82f,0.82f,0.98f,0.96f,0.97f,1.14f,1.40f,1.19f,0.94f,1.00f,1.07f,1.37f,1.21f,1.48f,1.30f,1.57f,1.61f,1.37f,0.86f,0.83f,0.91f,0.82f,0.82f,0.88f,0.89f,0.96f,1.14f,0.98f,0.70f,0.72f,0.73f,0.77f,0.76f,0.79f,0.70f,0.72f,0.71f,0.82f,0.77f,0.80f,0.74f,0.79f,0.80f,0.74f,0.87f,0.93f,0.85f,1.23f,1.02f,1.02f,0.93f,0.93f,0.87f,0.85f,1.30f,1.35f,1.07f,1.38f,1.11f,0.94f,1.47f,1.71f,1.56f,1.97f,1.88f,1.92f,1.79f,1.79f,1.59f,1.60f,1.30f,1.35f,1.56f,1.37f,1.38f,1.59f,1.60f,1.79f,1.92f,1.79f,1.48f,1.57f,1.72f,1.61f,1.78f,1.79f,1.93f,1.99f,1.90f,1.86f,1.78f,1.86f,1.93f,1.99f,1.97f,1.90f,1.79f,1.72f,0.94f,1.07f,1.00f,1.37f,1.21f,1.30f,0.86f,0.91f,0.83f,1.14f,0.98f,0.96f,0.82f,0.88f,0.89f,0.79f,0.76f,0.73f,1.07f,0.94f,1.11f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f},
		{1.74f,1.57f,1.76f,1.33f,1.54f,1.71f,0.94f,1.05f,0.99f,1.26f,1.16f,1.46f,1.60f,1.34f,1.46f,1.59f,1.37f,1.37f,0.97f,1.11f,0.96f,1.10f,0.95f,0.94f,1.08f,0.89f,0.82f,0.88f,0.72f,0.76f,0.75f,0.80f,0.80f,0.88f,0.87f,0.91f,0.83f,0.87f,0.72f,0.76f,0.74f,0.83f,0.84f,0.78f,0.79f,0.96f,0.89f,0.92f,0.98f,1.23f,1.05f,0.86f,0.92f,0.95f,1.11f,0.98f,1.22f,1.03f,1.34f,1.42f,1.14f,0.79f,0.77f,0.84f,0.78f,0.76f,0.82f,0.82f,0.89f,0.97f,0.90f,0.70f,0.71f,0.71f,0.73f,0.72f,0.74f,0.73f,0.76f,0.72f,0.86f,0.81f,0.82f,0.76f,0.79f,0.77f,0.73f,0.90f,0.95f,0.86f,1.18f,1.03f,0.98f,0.92f,0.90f,0.83f,0.84f,1.19f,1.17f,0.98f,1.15f,0.97f,0.89f,1.42f,1.65f,1.44f,1.93f,1.83f,1.81f,1.67f,1.61f,1.36f,1.41f,1.32f,1.45f,1.58f,1.57f,1.53f,1.74f,1.70f,1.88f,1.94f,1.81f,1.69f,1.77f,1.87f,1.79f,1.89f,1.92f,1.98f,1.99f,1.98f,1.89f,1.65f,1.80f,1.82f,1.91f,1.94f,1.75f,1.61f,1.50f,1.07f,1.34f,1.27f,1.60f,1.45f,1.55f,0.93f,0.99f,0.90f,1.35f,1.18f,1.07f,0.87f,0.93f,0.96f,0.85f,0.82f,0.77f,1.15f,0.99f,1.27f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f},
		{1.86f,1.71f,1.82f,1.48f,1.62f,1.71f,0.98f,1.20f,1.05f,1.34f,1.17f,1.34f,1.53f,1.27f,1.46f,1.77f,1.60f,1.57f,1.16f,1.38f,1.12f,1.35f,1.06f,1.00f,1.28f,0.97f,0.89f,0.95f,0.76f,0.81f,0.79f,0.86f,0.85f,0.92f,0.93f,0.93f,0.85f,0.87f,0.74f,0.78f,0.74f,0.79f,0.82f,0.76f,0.79f,0.96f,0.85f,0.90f,0.94f,1.09f,0.99f,0.81f,0.85f,0.89f,0.95f,0.90f,0.99f,0.94f,1.10f,1.24f,0.98f,0.75f,0.73f,0.78f,0.74f,0.72f,0.77f,0.76f,0.82f,0.89f,0.83f,0.73f,0.71f,0.71f,0.71f,0.70f,0.72f,0.77f,0.80f,0.74f,0.90f,0.85f,0.84f,0.78f,0.79f,0.75f,0.73f,0.92f,0.95f,0.86f,1.05f,0.99f,0.94f,0.90f,0.86f,0.79f,0.81f,1.00f,0.98f,0.91f,0.96f,0.89f,0.83f,1.27f,1.50f,1.23f,1.80f,1.69f,1.63f,1.46f,1.37f,1.09f,1.16f,1.24f,1.44f,1.49f,1.69f,1.59f,1.80f,1.69f,1.87f,1.86f,1.72f,1.82f,1.91f,1.94f,1.92f,1.95f,1.99f,1.98f,1.91f,1.97f,1.89f,1.51f,1.72f,1.67f,1.77f,1.86f,1.55f,1.41f,1.25f,1.33f,1.58f,1.50f,1.80f,1.63f,1.74f,1.04f,1.21f,0.97f,1.48f,1.37f,1.21f,0.93f,0.97f,1.05f,0.92f,0.88f,0.84f,1.14f,1.02f,1.34f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f},
		{1.94f,1.84f,1.87f,1.64f,1.71f,1.71f,1.14f,1.38f,1.19f,1.46f,1.23f,1.26f,1.48f,1.26f,1.50f,1.91f,1.80f,1.76f,1.41f,1.61f,1.39f,1.59f,1.33f,1.24f,1.51f,1.18f,0.97f,1.11f,0.82f,0.88f,0.86f,0.94f,0.92f,0.99f,1.03f,0.98f,0.91f,0.90f,0.79f,0.84f,0.77f,0.79f,0.84f,0.77f,0.83f,0.99f,0.85f,0.91f,0.92f,1.02f,1.00f,0.79f,0.80f,0.86f,0.88f,0.84f,0.92f,0.88f,0.97f,1.10f,0.94f,0.74f,0.71f,0.74f,0.72f,0.70f,0.73f,0.72f,0.76f,0.82f,0.77f,0.77f,0.73f,0.74f,0.71f,0.70f,0.73f,0.83f,0.85f,0.78f,0.92f,0.88f,0.86f,0.81f,0.79f,0.74f,0.75f,0.92f,0.93f,0.85f,0.96f,0.94f,0.88f,0.86f,0.81f,0.75f,0.79f,0.93f,0.90f,0.85f,0.88f,0.82f,0.77f,1.05f,1.27f,0.99f,1.60f,1.47f,1.39f,1.20f,1.11f,0.95f,0.97f,1.08f,1.33f,1.31f,1.70f,1.55f,1.76f,1.57f,1.76f,1.70f,1.54f,1.85f,1.97f,1.91f,1.99f,1.97f,1.99f,1.91f,1.77f,1.88f,1.85f,1.39f,1.64f,1.51f,1.58f,1.74f,1.32f,1.22f,1.01f,1.54f,1.76f,1.65f,1.93f,1.70f,1.85f,1.28f,1.39f,1.09f,1.52f,1.48f,1.26f,0.97f,0.99f,1.18f,1.00f,0.93f,0.90f,1.05f,1.01f,1.31f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f},
		{1.97f,1.92f,1.88f,1.79f,1.79f,1.71f,1.37f,1.59f,1.38f,1.60f,1.35f,1.23f,1.47f,1.30f,1.56f,1.99f,1.93f,1.90f,1.60f,1.78f,1.61f,1.79f,1.57f,1.48f,1.72f,1.40f,1.14f,1.37f,0.89f,0.96f,0.94f,1.07f,1.00f,1.21f,1.30f,1.14f,0.98f,0.96f,0.86f,0.91f,0.83f,0.82f,0.88f,0.82f,0.89f,1.11f,0.87f,0.94f,0.93f,1.02f,1.07f,0.80f,0.79f,0.85f,0.82f,0.80f,0.87f,0.85f,0.93f,1.02f,0.93f,0.77f,0.72f,0.74f,0.71f,0.70f,0.70f,0.71f,0.72f,0.77f,0.74f,0.82f,0.76f,0.79f,0.72f,0.73f,0.76f,0.89f,0.89f,0.82f,0.93f,0.91f,0.86f,0.83f,0.79f,0.73f,0.76f,0.91f,0.89f,0.83f,0.89f,0.89f,0.82f,0.82f,0.76f,0.72f,0.76f,0.86f,0.83f,0.79f,0.82f,0.76f,0.73f,0.94f,1.00f,0.91f,1.37f,1.21f,1.14f,0.98f,0.96f,0.88f,0.89f,0.96f,1.14f,1.07f,1.60f,1.40f,1.61f,1.37f,1.57f,1.48f,1.30f,1.78f,1.93f,1.79f,1.99f,1.92f,1.90f,1.79f,1.59f,1.72f,1.79f,1.30f,1.56f,1.35f,1.38f,1.60f,1.11f,1.07f,0.94f,1.68f,1.86f,1.71f,1.97f,1.68f,1.86f,1.44f,1.49f,1.22f,1.44f,1.49f,1.22f,0.99f,0.99f,1.23f,1.19f,0.98f,0.97f,0.97f,0.98f,1.19f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f},
		{1.94f,1.97f,1.87f,1.91f,1.85f,1.71f,1.60f,1.77f,1.58f,1.74f,1.51f,1.26f,1.48f,1.39f,1.64f,1.99f,1.97f,1.99f,1.70f,1.85f,1.76f,1.91f,1.76f,1.70f,1.88f,1.55f,1.33f,1.57f,0.96f,1.08f,1.05f,1.31f,1.27f,1.47f,1.54f,1.39f,1.20f,1.11f,0.93f,0.99f,0.90f,0.88f,0.95f,0.88f,0.97f,1.32f,0.92f,1.01f,0.97f,1.10f,1.22f,0.84f,0.80f,0.88f,0.79f,0.79f,0.85f,0.86f,0.92f,1.02f,0.94f,0.82f,0.76f,0.77f,0.72f,0.73f,0.70f,0.72f,0.71f,0.74f,0.74f,0.88f,0.81f,0.85f,0.75f,0.77f,0.82f,0.94f,0.93f,0.86f,0.92f,0.92f,0.86f,0.85f,0.79f,0.74f,0.79f,0.88f,0.85f,0.81f,0.82f,0.83f,0.77f,0.78f,0.73f,0.71f,0.75f,0.79f,0.77f,0.74f,0.77f,0.73f,0.70f,0.86f,0.92f,0.84f,1.14f,0.99f,0.98f,0.91f,0.90f,0.84f,0.83f,0.88f,0.97f,0.94f,1.41f,1.18f,1.39f,1.11f,1.33f,1.24f,1.03f,1.61f,1.80f,1.59f,1.91f,1.84f,1.76f,1.64f,1.38f,1.51f,1.71f,1.26f,1.50f,1.23f,1.19f,1.46f,0.99f,1.00f,0.91f,1.70f,1.85f,1.65f,1.93f,1.54f,1.76f,1.52f,1.48f,1.26f,1.28f,1.39f,1.09f,0.99f,0.97f,1.18f,1.31f,1.01f,1.05f,0.90f,0.93f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f},
		{1.86f,1.95f,1.82f,1.98f,1.89f,1.71f,1.80f,1.91f,1.77f,1.86f,1.67f,1.34f,1.53f,1.51f,1.72f,1.92f,1.91f,1.99f,1.69f,1.82f,1.80f,1.94f,1.87f,1.86f,1.97f,1.59f,1.44f,1.69f,1.05f,1.24f,1.27f,1.49f,1.50f,1.69f,1.72f,1.63f,1.46f,1.37f,1.00f,1.23f,0.98f,0.95f,1.09f,0.96f,1.16f,1.55f,0.99f,1.25f,1.10f,1.24f,1.41f,0.90f,0.85f,0.94f,0.79f,0.81f,0.85f,0.89f,0.94f,1.09f,0.98f,0.89f,0.82f,0.83f,0.74f,0.77f,0.72f,0.76f,0.73f,0.75f,0.78f,0.94f,0.86f,0.91f,0.79f,0.83f,0.89f,0.99f,0.95f,0.90f,0.90f,0.92f,0.84f,0.86f,0.79f,0.75f,0.81f,0.85f,0.80f,0.78f,0.76f,0.77f,0.73f,0.74f,0.71f,0.71f,0.73f,0.74f,0.74f,0.71f,0.76f,0.72f,0.70f,0.79f,0.85f,0.78f,0.98f,0.92f,0.93f,0.85f,0.87f,0.82f,0.79f,0.81f,0.89f,0.86f,1.16f,0.97f,1.12f,0.95f,1.06f,1.00f,0.93f,1.38f,1.60f,1.35f,1.77f,1.71f,1.57f,1.48f,1.20f,1.28f,1.62f,1.27f,1.46f,1.17f,1.05f,1.34f,0.96f,0.99f,0.90f,1.63f,1.74f,1.50f,1.80f,1.33f,1.58f,1.48f,1.37f,1.21f,1.04f,1.21f,0.97f,0.97f,0.93f,1.05f,1.34f,1.02f,1.14f,0.84f,0.88f,0.92f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f},
		{1.74f,1.89f,1.76f,1.98f,1.89f,1.71f,1.93f,1.99f,1.91f,1.94f,1.82f,1.46f,1.60f,1.65f,1.80f,1.79f,1.77f,1.92f,1.57f,1.69f,1.74f,1.87f,1.88f,1.94f,1.98f,1.53f,1.45f,1.70f,1.18f,1.32f,1.42f,1.58f,1.65f,1.83f,1.81f,1.81f,1.67f,1.61f,1.19f,1.44f,1.17f,1.11f,1.36f,1.15f,1.41f,1.75f,1.22f,1.50f,1.34f,1.42f,1.61f,0.98f,0.92f,1.03f,0.83f,0.86f,0.89f,0.95f,0.98f,1.23f,1.14f,0.97f,0.89f,0.90f,0.78f,0.82f,0.76f,0.82f,0.77f,0.79f,0.84f,0.98f,0.90f,0.98f,0.83f,0.89f,0.97f,1.03f,0.95f,0.92f,0.86f,0.90f,0.82f,0.86f,0.79f,0.77f,0.84f,0.81f,0.76f,0.76f,0.72f,0.73f,0.70f,0.72f,0.71f,0.73f,0.73f,0.72f,0.74f,0.71f,0.78f,0.74f,0.72f,0.75f,0.80f,0.76f,0.94f,0.88f,0.91f,0.83f,0.87f,0.84f,0.79f,0.76f,0.82f,0.80f,0.97f,0.89f,0.96f,0.88f,0.95f,0.94f,0.87f,1.11f,1.37f,1.10f,1.59f,1.57f,1.37f,1.33f,1.05f,1.08f,1.54f,1.34f,1.46f,1.16f,0.99f,1.26f,0.96f,1.05f,0.92f,1.45f,1.55f,1.27f,1.60f,1.07f,1.34f,1.35f,1.18f,1.07f,0.93f,0.99f,0.90f,0.93f,0.87f,0.96f,1.27f,0.99f,1.15f,0.77f,0.82f,0.85f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f},
		{1.60f,1.78f,1.68f,1.93f,1.86f,1.71f,1.97f,1.99f,1.99f,1.97f,1.93f,1.60f,1.68f,1.78f,1.86f,1.61f,1.57f,1.79f,1.37f,1.48f,1.59f,1.72f,1.79f,1.92f,1.90f,1.38f,1.35f,1.60f,1.23f,1.30f,1.47f,1.56f,1.71f,1.88f,1.79f,1.92f,1.79f,1.79f,1.30f,1.56f,1.35f,1.37f,1.59f,1.38f,1.60f,1.90f,1.48f,1.72f,1.57f,1.61f,1.79f,1.21f,1.00f,1.30f,0.89f,0.94f,0.96f,1.07f,1.14f,1.40f,1.37f,1.14f,0.96f,0.98f,0.82f,0.88f,0.82f,0.89f,0.83f,0.86f,0.91f,1.02f,0.93f,1.07f,0.87f,0.94f,1.11f,1.02f,0.93f,0.93f,0.82f,0.87f,0.80f,0.85f,0.79f,0.80f,0.85f,0.77f,0.72f,0.74f,0.71f,0.70f,0.70f,0.71f,0.72f,0.77f,0.74f,0.72f,0.76f,0.73f,0.82f,0.79f,0.76f,0.73f,0.79f,0.76f,0.93f,0.86f,0.91f,0.83f,0.89f,0.89f,0.82f,0.72f,0.76f,0.76f,0.89f,0.82f,0.89f,0.82f,0.89f,0.91f,0.83f,0.96f,1.14f,0.97f,1.40f,1.44f,1.19f,1.22f,0.99f,0.98f,1.49f,1.44f,1.49f,1.22f,0.99f,1.23f,0.98f,1.19f,0.97f,1.21f,1.30f,1.00f,1.37f,0.94f,1.07f,1.14f,0.98f,0.96f,0.86f,0.91f,0.83f,0.88f,0.82f,0.89f,1.11f,0.94f,1.07f,0.73f,0.76f,0.79f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f},
		{1.46f,1.65f,1.60f,1.82f,1.80f,1.71f,1.93f,1.91f,1.99f,1.94f,1.98f,1.74f,1.76f,1.89f,1.89f,1.42f,1.34f,1.61f,1.11f,1.22f,1.36f,1.50f,1.61f,1.81f,1.75f,1.15f,1.17f,1.41f,1.18f,1.19f,1.42f,1.44f,1.65f,1.83f,1.67f,1.94f,1.81f,1.88f,1.32f,1.58f,1.45f,1.57f,1.74f,1.53f,1.70f,1.98f,1.69f,1.87f,1.77f,1.79f,1.92f,1.45f,1.27f,1.55f,0.97f,1.07f,1.11f,1.34f,1.37f,1.59f,1.60f,1.35f,1.07f,1.18f,0.86f,0.93f,0.87f,0.96f,0.90f,0.93f,0.99f,1.03f,0.95f,1.15f,0.90f,0.99f,1.27f,0.98f,0.90f,0.92f,0.78f,0.83f,0.77f,0.84f,0.79f,0.82f,0.86f,0.73f,0.71f,0.73f,0.72f,0.70f,0.73f,0.72f,0.76f,0.81f,0.76f,0.76f,0.82f,0.77f,0.89f,0.85f,0.82f,0.75f,0.80f,0.80f,0.94f,0.88f,0.94f,0.87f,0.95f,0.96f,0.88f,0.72f,0.74f,0.76f,0.83f,0.78f,0.84f,0.79f,0.87f,0.91f,0.83f,0.89f,0.98f,0.92f,1.23f,1.34f,1.05f,1.16f,0.99f,0.96f,1.46f,1.57f,1.54f,1.33f,1.05f,1.26f,1.08f,1.37f,1.10f,0.98f,1.03f,0.92f,1.14f,0.86f,0.95f,0.97f,0.90f,0.89f,0.79f,0.84f,0.77f,0.82f,0.76f,0.82f,0.97f,0.89f,0.98f,0.71f,0.72f,0.74f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f},
		{1.34f,1.51f,1.53f,1.67f,1.72f,1.71f,1.80f,1.77f,1.91f,1.86f,1.98f,1.86f,1.82f,1.95f,1.89f,1.24f,1.10f,1.41f,0.95f,0.99f,1.09f,1.25f,1.37f,1.63f,1.55f,0.96f,0.98f,1.16f,1.05f,1.00f,1.27f,1.23f,1.50f,1.69f,1.46f,1.86f,1.72f,1.87f,1.24f,1.49f,1.44f,1.69f,1.80f,1.59f,1.69f,1.97f,1.82f,1.94f,1.91f,1.92f,1.99f,1.63f,1.50f,1.74f,1.16f,1.33f,1.38f,1.58f,1.60f,1.77f,1.80f,1.48f,1.21f,1.37f,0.90f,0.97f,0.93f,1.05f,0.97f,1.04f,1.21f,0.99f,0.95f,1.14f,0.92f,1.02f,1.34f,0.94f,0.86f,0.90f,0.74f,0.79f,0.75f,0.81f,0.79f,0.84f,0.86f,0.71f,0.71f,0.73f,0.76f,0.73f,0.77f,0.74f,0.80f,0.85f,0.78f,0.81f,0.89f,0.84f,0.97f,0.92f,0.88f,0.79f,0.85f,0.86f,0.98f,0.92f,1.00f,0.93f,1.06f,1.12f,0.95f,0.74f,0.74f,0.78f,0.79f,0.76f,0.82f,0.79f,0.87f,0.93f,0.85f,0.85f,0.94f,0.90f,1.09f,1.27f,0.99f,1.17f,1.05f,0.96f,1.46f,1.71f,1.62f,1.48f,1.20f,1.34f,1.28f,1.57f,1.35f,0.90f,0.94f,0.85f,0.98f,0.81f,0.89f,0.89f,0.83f,0.82f,0.75f,0.78f,0.73f,0.77f,0.72f,0.76f,0.89f,0.83f,0.91f,0.71f,0.70f,0.72f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f},
		{1.26f,1.39f,1.48f,1.51f,1.64f,1.71f,1.60f,1.58f,1.77f,1.74f,1.91f,1.94f,1.87f,1.97f,1.85f,1.10f,0.97f,1.22f,0.88f,0.92f,0.95f,1.01f,1.11f,1.39f,1.32f,0.88f,0.90f,0.97f,0.96f,0.93f,1.05f,0.99f,1.27f,1.47f,1.20f,1.70f,1.54f,1.76f,1.08f,1.31f,1.33f,1.70f,1.76f,1.55f,1.57f,1.88f,1.85f,1.91f,1.97f,1.99f,1.99f,1.70f,1.65f,1.85f,1.41f,1.54f,1.61f,1.76f,1.80f,1.91f,1.93f,1.52f,1.26f,1.48f,0.92f,0.99f,0.97f,1.18f,1.09f,1.28f,1.39f,0.94f,0.93f,1.05f,0.92f,1.01f,1.31f,0.88f,0.81f,0.86f,0.72f,0.75f,0.74f,0.79f,0.79f,0.86f,0.85f,0.71f,0.73f,0.75f,0.82f,0.77f,0.83f,0.78f,0.85f,0.88f,0.81f,0.88f,0.97f,0.90f,1.18f,1.00f,0.93f,0.86f,0.92f,0.94f,1.14f,0.99f,1.24f,1.03f,1.33f,1.39f,1.11f,0.79f,0.77f,0.84f,0.79f,0.77f,0.84f,0.83f,0.90f,0.98f,0.91f,0.85f,0.92f,0.91f,1.02f,1.26f,1.00f,1.23f,1.19f,0.99f,1.50f,1.84f,1.71f,1.64f,1.38f,1.46f,1.51f,1.76f,1.59f,0.84f,0.88f,0.80f,0.94f,0.79f,0.86f,0.82f,0.77f,0.76f,0.74f,0.74f,0.71f,0.73f,0.70f,0.72f,0.82f,0.77f,0.85f,0.74f,0.70f,0.73f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f,1.00f}
	};

}
