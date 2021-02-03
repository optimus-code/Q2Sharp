using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Q2Sharp.Render
{
    public static class Anorms
    {
        public static readonly float[][] VERTEXNORMALS = new[]{new[]{-0.525731F, 0F, 0.850651F}, new[]{-0.442863F, 0.238856F, 0.864188F}, new[]{-0.295242F, 0F, 0.955423F}, new[]{-0.309017F, 0.5F, 0.809017F}, new[]{-0.16246F, 0.262866F, 0.951056F}, new[]{0F, 0F, 1F}, new[]{0F, 0.850651F, 0.525731F}, new[]{-0.147621F, 0.716567F, 0.681718F}, new[]{0.147621F, 0.716567F, 0.681718F}, new[]{0F, 0.525731F, 0.850651F}, new[]{0.309017F, 0.5F, 0.809017F}, new[]{0.525731F, 0F, 0.850651F}, new[]{0.295242F, 0F, 0.955423F}, new[]{0.442863F, 0.238856F, 0.864188F}, new[]{0.16246F, 0.262866F, 0.951056F}, new[]{-0.681718F, 0.147621F, 0.716567F}, new[]{-0.809017F, 0.309017F, 0.5F}, new[]{-0.587785F, 0.425325F, 0.688191F}, new[]{-0.850651F, 0.525731F, 0F}, new[]{-0.864188F, 0.442863F, 0.238856F}, new[]{-0.716567F, 0.681718F, 0.147621F}, new[]{-0.688191F, 0.587785F, 0.425325F}, new[]{-0.5F, 0.809017F, 0.309017F}, new[]{-0.238856F, 0.864188F, 0.442863F}, new[]{-0.425325F, 0.688191F, 0.587785F}, new[]{-0.716567F, 0.681718F, -0.147621F}, new[]{-0.5F, 0.809017F, -0.309017F}, new[]{-0.525731F, 0.850651F, 0F}, new[]{0F, 0.850651F, -0.525731F}, new[]{-0.238856F, 0.864188F, -0.442863F}, new[]{0F, 0.955423F, -0.295242F}, new[]{-0.262866F, 0.951056F, -0.16246F}, new[]{0F, 1F, 0F}, new[]{0F, 0.955423F, 0.295242F}, new[]{-0.262866F, 0.951056F, 0.16246F}, new[]{0.238856F, 0.864188F, 0.442863F}, new[]{0.262866F, 0.951056F, 0.16246F}, new[]{0.5F, 0.809017F, 0.309017F}, new[]{0.238856F, 0.864188F, -0.442863F}, new[]{0.262866F, 0.951056F, -0.16246F}, new[]{0.5F, 0.809017F, -0.309017F}, new[]{0.850651F, 0.525731F, 0F}, new[]{0.716567F, 0.681718F, 0.147621F}, new[]{0.716567F, 0.681718F, -0.147621F}, new[]{0.525731F, 0.850651F, 0F}, new[]{0.425325F, 0.688191F, 0.587785F}, new[]{0.864188F, 0.442863F, 0.238856F}, new[]{0.688191F, 0.587785F, 0.425325F}, new[]{0.809017F, 0.309017F, 0.5F}, new[]{0.681718F, 0.147621F, 0.716567F}, new[]{0.587785F, 0.425325F, 0.688191F}, new[]{0.955423F, 0.295242F, 0F}, new[]{1F, 0F, 0F}, new[]{0.951056F, 0.16246F, 0.262866F}, new[]{0.850651F, -0.525731F, 0F}, new[]{0.955423F, -0.295242F, 0F}, new[]{0.864188F, -0.442863F, 0.238856F}, new[]{0.951056F, -0.16246F, 0.262866F}, new[]{0.809017F, -0.309017F, 0.5F}, new[]{0.681718F, -0.147621F, 0.716567F}, new[]{0.850651F, 0F, 0.525731F}, new[]{0.864188F, 0.442863F, -0.238856F}, new[]{0.809017F, 0.309017F, -0.5F}, new[]{0.951056F, 0.16246F, -0.262866F}, new[]{0.525731F, 0F, -0.850651F}, new[]{0.681718F, 0.147621F, -0.716567F}, new[]{0.681718F, -0.147621F, -0.716567F}, new[]{0.850651F, 0F, -0.525731F}, new[]{0.809017F, -0.309017F, -0.5F}, new[]{0.864188F, -0.442863F, -0.238856F}, new[]{0.951056F, -0.16246F, -0.262866F}, new[]{0.147621F, 0.716567F, -0.681718F}, new[]{0.309017F, 0.5F, -0.809017F}, new[]{0.425325F, 0.688191F, -0.587785F}, new[]{0.442863F, 0.238856F, -0.864188F}, new[]{0.587785F, 0.425325F, -0.688191F}, new[]{0.688191F, 0.587785F, -0.425325F}, new[]{-0.147621F, 0.716567F, -0.681718F}, new[]{-0.309017F, 0.5F, -0.809017F}, new[]{0F, 0.525731F, -0.850651F}, new[]{-0.525731F, 0F, -0.850651F}, new[]{-0.442863F, 0.238856F, -0.864188F}, new[]{-0.295242F, 0F, -0.955423F}, new[]{-0.16246F, 0.262866F, -0.951056F}, new[]{0F, 0F, -1F}, new[]{0.295242F, 0F, -0.955423F}, new[]{0.16246F, 0.262866F, -0.951056F}, new[]{-0.442863F, -0.238856F, -0.864188F}, new[]{-0.309017F, -0.5F, -0.809017F}, new[]{-0.16246F, -0.262866F, -0.951056F}, new[]{0F, -0.850651F, -0.525731F}, new[]{-0.147621F, -0.716567F, -0.681718F}, new[]{0.147621F, -0.716567F, -0.681718F}, new[]{0F, -0.525731F, -0.850651F}, new[]{0.309017F, -0.5F, -0.809017F}, new[]{0.442863F, -0.238856F, -0.864188F}, new[]{0.16246F, -0.262866F, -0.951056F}, new[]{0.238856F, -0.864188F, -0.442863F}, new[]{0.5F, -0.809017F, -0.309017F}, new[]{0.425325F, -0.688191F, -0.587785F}, new[]{0.716567F, -0.681718F, -0.147621F}, new[]{0.688191F, -0.587785F, -0.425325F}, new[]{0.587785F, -0.425325F, -0.688191F}, new[]{0F, -0.955423F, -0.295242F}, new[]{0F, -1F, 0F}, new[]{0.262866F, -0.951056F, -0.16246F}, new[]{0F, -0.850651F, 0.525731F}, new[]{0F, -0.955423F, 0.295242F}, new[]{0.238856F, -0.864188F, 0.442863F}, new[]{0.262866F, -0.951056F, 0.16246F}, new[]{0.5F, -0.809017F, 0.309017F}, new[]{0.716567F, -0.681718F, 0.147621F}, new[]{0.525731F, -0.850651F, 0F}, new[]{-0.238856F, -0.864188F, -0.442863F}, new[]{-0.5F, -0.809017F, -0.309017F}, new[]{-0.262866F, -0.951056F, -0.16246F}, new[]{-0.850651F, -0.525731F, 0F}, new[]{-0.716567F, -0.681718F, -0.147621F}, new[]{-0.716567F, -0.681718F, 0.147621F}, new[]{-0.525731F, -0.850651F, 0F}, new[]{-0.5F, -0.809017F, 0.309017F}, new[]{-0.238856F, -0.864188F, 0.442863F}, new[]{-0.262866F, -0.951056F, 0.16246F}, new[]{-0.864188F, -0.442863F, 0.238856F}, new[]{-0.809017F, -0.309017F, 0.5F}, new[]{-0.688191F, -0.587785F, 0.425325F}, new[]{-0.681718F, -0.147621F, 0.716567F}, new[]{-0.442863F, -0.238856F, 0.864188F}, new[]{-0.587785F, -0.425325F, 0.688191F}, new[]{-0.309017F, -0.5F, 0.809017F}, new[]{-0.147621F, -0.716567F, 0.681718F}, new[]{-0.425325F, -0.688191F, 0.587785F}, new[]{-0.16246F, -0.262866F, 0.951056F}, new[]{0.442863F, -0.238856F, 0.864188F}, new[]{0.16246F, -0.262866F, 0.951056F}, new[]{0.309017F, -0.5F, 0.809017F}, new[]{0.147621F, -0.716567F, 0.681718F}, new[]{0F, -0.525731F, 0.850651F}, new[]{0.425325F, -0.688191F, 0.587785F}, new[]{0.587785F, -0.425325F, 0.688191F}, new[]{0.688191F, -0.587785F, 0.425325F}, new[]{-0.955423F, 0.295242F, 0F}, new[]{-0.951056F, 0.16246F, 0.262866F}, new[]{-1F, 0F, 0F}, new[]{-0.850651F, 0F, 0.525731F}, new[]{-0.955423F, -0.295242F, 0F}, new[]{-0.951056F, -0.16246F, 0.262866F}, new[]{-0.864188F, 0.442863F, -0.238856F}, new[]{-0.951056F, 0.16246F, -0.262866F}, new[]{-0.809017F, 0.309017F, -0.5F}, new[]{-0.864188F, -0.442863F, -0.238856F}, new[]{-0.951056F, -0.16246F, -0.262866F}, new[]{-0.809017F, -0.309017F, -0.5F}, new[]{-0.681718F, 0.147621F, -0.716567F}, new[]{-0.681718F, -0.147621F, -0.716567F}, new[]{-0.850651F, 0F, -0.525731F}, new[]{-0.688191F, 0.587785F, -0.425325F}, new[]{-0.587785F, 0.425325F, -0.688191F}, new[]{-0.425325F, 0.688191F, -0.587785F}, new[]{-0.425325F, -0.688191F, -0.587785F}, new[]{-0.587785F, -0.425325F, -0.688191F}, new[]{-0.688191F, -0.587785F, -0.425325F}};
        public static readonly float[][] VERTEXNORMAL_DOTS = new[]{new[]{1.23F, 1.3F, 1.47F, 1.35F, 1.56F, 1.71F, 1.37F, 1.38F, 1.59F, 1.6F, 1.79F, 1.97F, 1.88F, 1.92F, 1.79F, 1.02F, 0.93F, 1.07F, 0.82F, 0.87F, 0.88F, 0.94F, 0.96F, 1.14F, 1.11F, 0.82F, 0.83F, 0.89F, 0.89F, 0.86F, 0.94F, 0.91F, 1F, 1.21F, 0.98F, 1.48F, 1.3F, 1.57F, 0.96F, 1.07F, 1.14F, 1.6F, 1.61F, 1.4F, 1.37F, 1.72F, 1.78F, 1.79F, 1.93F, 1.99F, 1.9F, 1.68F, 1.71F, 1.86F, 1.6F, 1.68F, 1.78F, 1.86F, 1.93F, 1.99F, 1.97F, 1.44F, 1.22F, 1.49F, 0.93F, 0.99F, 0.99F, 1.23F, 1.22F, 1.44F, 1.49F, 0.89F, 0.89F, 0.97F, 0.91F, 0.98F, 1.19F, 0.82F, 0.76F, 0.82F, 0.71F, 0.72F, 0.73F, 0.76F, 0.79F, 0.86F, 0.83F, 0.72F, 0.76F, 0.76F, 0.89F, 0.82F, 0.89F, 0.82F, 0.89F, 0.91F, 0.83F, 0.96F, 1.14F, 0.97F, 1.4F, 1.19F, 0.98F, 0.94F, 1F, 1.07F, 1.37F, 1.21F, 1.48F, 1.3F, 1.57F, 1.61F, 1.37F, 0.86F, 0.83F, 0.91F, 0.82F, 0.82F, 0.88F, 0.89F, 0.96F, 1.14F, 0.98F, 0.87F, 0.93F, 0.94F, 1.02F, 1.3F, 1.07F, 1.35F, 1.38F, 1.11F, 1.56F, 1.92F, 1.79F, 1.79F, 1.59F, 1.6F, 1.72F, 1.9F, 1.79F, 0.8F, 0.85F, 0.79F, 0.93F, 0.8F, 0.85F, 0.77F, 0.74F, 0.72F, 0.77F, 0.74F, 0.72F, 0.7F, 0.7F, 0.71F, 0.76F, 0.73F, 0.79F, 0.79F, 0.73F, 0.76F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F}, new[]{1.26F, 1.26F, 1.48F, 1.23F, 1.5F, 1.71F, 1.14F, 1.19F, 1.38F, 1.46F, 1.64F, 1.94F, 1.87F, 1.84F, 1.71F, 1.02F, 0.92F, 1F, 0.79F, 0.85F, 0.84F, 0.91F, 0.9F, 0.98F, 0.99F, 0.77F, 0.77F, 0.83F, 0.82F, 0.79F, 0.86F, 0.84F, 0.92F, 0.99F, 0.91F, 1.24F, 1.03F, 1.33F, 0.88F, 0.94F, 0.97F, 1.41F, 1.39F, 1.18F, 1.11F, 1.51F, 1.61F, 1.59F, 1.8F, 1.91F, 1.76F, 1.54F, 1.65F, 1.76F, 1.7F, 1.7F, 1.85F, 1.85F, 1.97F, 1.99F, 1.93F, 1.28F, 1.09F, 1.39F, 0.92F, 0.97F, 0.99F, 1.18F, 1.26F, 1.52F, 1.48F, 0.83F, 0.85F, 0.9F, 0.88F, 0.93F, 1F, 0.77F, 0.73F, 0.78F, 0.72F, 0.71F, 0.74F, 0.75F, 0.79F, 0.86F, 0.81F, 0.75F, 0.81F, 0.79F, 0.96F, 0.88F, 0.94F, 0.86F, 0.93F, 0.92F, 0.85F, 1.08F, 1.33F, 1.05F, 1.55F, 1.31F, 1.01F, 1.05F, 1.27F, 1.31F, 1.6F, 1.47F, 1.7F, 1.54F, 1.76F, 1.76F, 1.57F, 0.93F, 0.9F, 0.99F, 0.88F, 0.88F, 0.95F, 0.97F, 1.11F, 1.39F, 1.2F, 0.92F, 0.97F, 1.01F, 1.1F, 1.39F, 1.22F, 1.51F, 1.58F, 1.32F, 1.64F, 1.97F, 1.85F, 1.91F, 1.77F, 1.74F, 1.88F, 1.99F, 1.91F, 0.79F, 0.86F, 0.8F, 0.94F, 0.84F, 0.88F, 0.74F, 0.74F, 0.71F, 0.82F, 0.77F, 0.76F, 0.7F, 0.73F, 0.72F, 0.73F, 0.7F, 0.74F, 0.85F, 0.77F, 0.82F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F}, new[]{1.34F, 1.27F, 1.53F, 1.17F, 1.46F, 1.71F, 0.98F, 1.05F, 1.2F, 1.34F, 1.48F, 1.86F, 1.82F, 1.71F, 1.62F, 1.09F, 0.94F, 0.99F, 0.79F, 0.85F, 0.82F, 0.9F, 0.87F, 0.93F, 0.96F, 0.76F, 0.74F, 0.79F, 0.76F, 0.74F, 0.79F, 0.78F, 0.85F, 0.92F, 0.85F, 1F, 0.93F, 1.06F, 0.81F, 0.86F, 0.89F, 1.16F, 1.12F, 0.97F, 0.95F, 1.28F, 1.38F, 1.35F, 1.6F, 1.77F, 1.57F, 1.33F, 1.5F, 1.58F, 1.69F, 1.63F, 1.82F, 1.74F, 1.91F, 1.92F, 1.8F, 1.04F, 0.97F, 1.21F, 0.9F, 0.93F, 0.97F, 1.05F, 1.21F, 1.48F, 1.37F, 0.77F, 0.8F, 0.84F, 0.85F, 0.88F, 0.92F, 0.73F, 0.71F, 0.74F, 0.74F, 0.71F, 0.75F, 0.73F, 0.79F, 0.84F, 0.78F, 0.79F, 0.86F, 0.81F, 1.05F, 0.94F, 0.99F, 0.9F, 0.95F, 0.92F, 0.86F, 1.24F, 1.44F, 1.14F, 1.59F, 1.34F, 1.02F, 1.27F, 1.5F, 1.49F, 1.8F, 1.69F, 1.86F, 1.72F, 1.87F, 1.8F, 1.69F, 1F, 0.98F, 1.23F, 0.95F, 0.96F, 1.09F, 1.16F, 1.37F, 1.63F, 1.46F, 0.99F, 1.1F, 1.25F, 1.24F, 1.51F, 1.41F, 1.67F, 1.77F, 1.55F, 1.72F, 1.95F, 1.89F, 1.98F, 1.91F, 1.86F, 1.97F, 1.99F, 1.94F, 0.81F, 0.89F, 0.85F, 0.98F, 0.9F, 0.94F, 0.75F, 0.78F, 0.73F, 0.89F, 0.83F, 0.82F, 0.72F, 0.77F, 0.76F, 0.72F, 0.7F, 0.71F, 0.91F, 0.83F, 0.89F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F}, new[]{1.46F, 1.34F, 1.6F, 1.16F, 1.46F, 1.71F, 0.94F, 0.99F, 1.05F, 1.26F, 1.33F, 1.74F, 1.76F, 1.57F, 1.54F, 1.23F, 0.98F, 1.05F, 0.83F, 0.89F, 0.84F, 0.92F, 0.87F, 0.91F, 0.96F, 0.78F, 0.74F, 0.79F, 0.72F, 0.72F, 0.75F, 0.76F, 0.8F, 0.88F, 0.83F, 0.94F, 0.87F, 0.95F, 0.76F, 0.8F, 0.82F, 0.97F, 0.96F, 0.89F, 0.88F, 1.08F, 1.11F, 1.1F, 1.37F, 1.59F, 1.37F, 1.07F, 1.27F, 1.34F, 1.57F, 1.45F, 1.69F, 1.55F, 1.77F, 1.79F, 1.6F, 0.93F, 0.9F, 0.99F, 0.86F, 0.87F, 0.93F, 0.96F, 1.07F, 1.35F, 1.18F, 0.73F, 0.76F, 0.77F, 0.81F, 0.82F, 0.85F, 0.7F, 0.71F, 0.72F, 0.78F, 0.73F, 0.77F, 0.73F, 0.79F, 0.82F, 0.76F, 0.83F, 0.9F, 0.84F, 1.18F, 0.98F, 1.03F, 0.92F, 0.95F, 0.9F, 0.86F, 1.32F, 1.45F, 1.15F, 1.53F, 1.27F, 0.99F, 1.42F, 1.65F, 1.58F, 1.93F, 1.83F, 1.94F, 1.81F, 1.88F, 1.74F, 1.7F, 1.19F, 1.17F, 1.44F, 1.11F, 1.15F, 1.36F, 1.41F, 1.61F, 1.81F, 1.67F, 1.22F, 1.34F, 1.5F, 1.42F, 1.65F, 1.61F, 1.82F, 1.91F, 1.75F, 1.8F, 1.89F, 1.89F, 1.98F, 1.99F, 1.94F, 1.98F, 1.92F, 1.87F, 0.86F, 0.95F, 0.92F, 1.14F, 0.98F, 1.03F, 0.79F, 0.84F, 0.77F, 0.97F, 0.9F, 0.89F, 0.76F, 0.82F, 0.82F, 0.74F, 0.72F, 0.71F, 0.98F, 0.89F, 0.97F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F}, new[]{1.6F, 1.44F, 1.68F, 1.22F, 1.49F, 1.71F, 0.93F, 0.99F, 0.99F, 1.23F, 1.22F, 1.6F, 1.68F, 1.44F, 1.49F, 1.4F, 1.14F, 1.19F, 0.89F, 0.96F, 0.89F, 0.97F, 0.89F, 0.91F, 0.98F, 0.82F, 0.76F, 0.82F, 0.71F, 0.72F, 0.73F, 0.76F, 0.79F, 0.86F, 0.83F, 0.91F, 0.83F, 0.89F, 0.72F, 0.76F, 0.76F, 0.89F, 0.89F, 0.82F, 0.82F, 0.98F, 0.96F, 0.97F, 1.14F, 1.4F, 1.19F, 0.94F, 1F, 1.07F, 1.37F, 1.21F, 1.48F, 1.3F, 1.57F, 1.61F, 1.37F, 0.86F, 0.83F, 0.91F, 0.82F, 0.82F, 0.88F, 0.89F, 0.96F, 1.14F, 0.98F, 0.7F, 0.72F, 0.73F, 0.77F, 0.76F, 0.79F, 0.7F, 0.72F, 0.71F, 0.82F, 0.77F, 0.8F, 0.74F, 0.79F, 0.8F, 0.74F, 0.87F, 0.93F, 0.85F, 1.23F, 1.02F, 1.02F, 0.93F, 0.93F, 0.87F, 0.85F, 1.3F, 1.35F, 1.07F, 1.38F, 1.11F, 0.94F, 1.47F, 1.71F, 1.56F, 1.97F, 1.88F, 1.92F, 1.79F, 1.79F, 1.59F, 1.6F, 1.3F, 1.35F, 1.56F, 1.37F, 1.38F, 1.59F, 1.6F, 1.79F, 1.92F, 1.79F, 1.48F, 1.57F, 1.72F, 1.61F, 1.78F, 1.79F, 1.93F, 1.99F, 1.9F, 1.86F, 1.78F, 1.86F, 1.93F, 1.99F, 1.97F, 1.9F, 1.79F, 1.72F, 0.94F, 1.07F, 1F, 1.37F, 1.21F, 1.3F, 0.86F, 0.91F, 0.83F, 1.14F, 0.98F, 0.96F, 0.82F, 0.88F, 0.89F, 0.79F, 0.76F, 0.73F, 1.07F, 0.94F, 1.11F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F}, new[]{1.74F, 1.57F, 1.76F, 1.33F, 1.54F, 1.71F, 0.94F, 1.05F, 0.99F, 1.26F, 1.16F, 1.46F, 1.6F, 1.34F, 1.46F, 1.59F, 1.37F, 1.37F, 0.97F, 1.11F, 0.96F, 1.1F, 0.95F, 0.94F, 1.08F, 0.89F, 0.82F, 0.88F, 0.72F, 0.76F, 0.75F, 0.8F, 0.8F, 0.88F, 0.87F, 0.91F, 0.83F, 0.87F, 0.72F, 0.76F, 0.74F, 0.83F, 0.84F, 0.78F, 0.79F, 0.96F, 0.89F, 0.92F, 0.98F, 1.23F, 1.05F, 0.86F, 0.92F, 0.95F, 1.11F, 0.98F, 1.22F, 1.03F, 1.34F, 1.42F, 1.14F, 0.79F, 0.77F, 0.84F, 0.78F, 0.76F, 0.82F, 0.82F, 0.89F, 0.97F, 0.9F, 0.7F, 0.71F, 0.71F, 0.73F, 0.72F, 0.74F, 0.73F, 0.76F, 0.72F, 0.86F, 0.81F, 0.82F, 0.76F, 0.79F, 0.77F, 0.73F, 0.9F, 0.95F, 0.86F, 1.18F, 1.03F, 0.98F, 0.92F, 0.9F, 0.83F, 0.84F, 1.19F, 1.17F, 0.98F, 1.15F, 0.97F, 0.89F, 1.42F, 1.65F, 1.44F, 1.93F, 1.83F, 1.81F, 1.67F, 1.61F, 1.36F, 1.41F, 1.32F, 1.45F, 1.58F, 1.57F, 1.53F, 1.74F, 1.7F, 1.88F, 1.94F, 1.81F, 1.69F, 1.77F, 1.87F, 1.79F, 1.89F, 1.92F, 1.98F, 1.99F, 1.98F, 1.89F, 1.65F, 1.8F, 1.82F, 1.91F, 1.94F, 1.75F, 1.61F, 1.5F, 1.07F, 1.34F, 1.27F, 1.6F, 1.45F, 1.55F, 0.93F, 0.99F, 0.9F, 1.35F, 1.18F, 1.07F, 0.87F, 0.93F, 0.96F, 0.85F, 0.82F, 0.77F, 1.15F, 0.99F, 1.27F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F}, new[]{1.86F, 1.71F, 1.82F, 1.48F, 1.62F, 1.71F, 0.98F, 1.2F, 1.05F, 1.34F, 1.17F, 1.34F, 1.53F, 1.27F, 1.46F, 1.77F, 1.6F, 1.57F, 1.16F, 1.38F, 1.12F, 1.35F, 1.06F, 1F, 1.28F, 0.97F, 0.89F, 0.95F, 0.76F, 0.81F, 0.79F, 0.86F, 0.85F, 0.92F, 0.93F, 0.93F, 0.85F, 0.87F, 0.74F, 0.78F, 0.74F, 0.79F, 0.82F, 0.76F, 0.79F, 0.96F, 0.85F, 0.9F, 0.94F, 1.09F, 0.99F, 0.81F, 0.85F, 0.89F, 0.95F, 0.9F, 0.99F, 0.94F, 1.1F, 1.24F, 0.98F, 0.75F, 0.73F, 0.78F, 0.74F, 0.72F, 0.77F, 0.76F, 0.82F, 0.89F, 0.83F, 0.73F, 0.71F, 0.71F, 0.71F, 0.7F, 0.72F, 0.77F, 0.8F, 0.74F, 0.9F, 0.85F, 0.84F, 0.78F, 0.79F, 0.75F, 0.73F, 0.92F, 0.95F, 0.86F, 1.05F, 0.99F, 0.94F, 0.9F, 0.86F, 0.79F, 0.81F, 1F, 0.98F, 0.91F, 0.96F, 0.89F, 0.83F, 1.27F, 1.5F, 1.23F, 1.8F, 1.69F, 1.63F, 1.46F, 1.37F, 1.09F, 1.16F, 1.24F, 1.44F, 1.49F, 1.69F, 1.59F, 1.8F, 1.69F, 1.87F, 1.86F, 1.72F, 1.82F, 1.91F, 1.94F, 1.92F, 1.95F, 1.99F, 1.98F, 1.91F, 1.97F, 1.89F, 1.51F, 1.72F, 1.67F, 1.77F, 1.86F, 1.55F, 1.41F, 1.25F, 1.33F, 1.58F, 1.5F, 1.8F, 1.63F, 1.74F, 1.04F, 1.21F, 0.97F, 1.48F, 1.37F, 1.21F, 0.93F, 0.97F, 1.05F, 0.92F, 0.88F, 0.84F, 1.14F, 1.02F, 1.34F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F}, new[]{1.94F, 1.84F, 1.87F, 1.64F, 1.71F, 1.71F, 1.14F, 1.38F, 1.19F, 1.46F, 1.23F, 1.26F, 1.48F, 1.26F, 1.5F, 1.91F, 1.8F, 1.76F, 1.41F, 1.61F, 1.39F, 1.59F, 1.33F, 1.24F, 1.51F, 1.18F, 0.97F, 1.11F, 0.82F, 0.88F, 0.86F, 0.94F, 0.92F, 0.99F, 1.03F, 0.98F, 0.91F, 0.9F, 0.79F, 0.84F, 0.77F, 0.79F, 0.84F, 0.77F, 0.83F, 0.99F, 0.85F, 0.91F, 0.92F, 1.02F, 1F, 0.79F, 0.8F, 0.86F, 0.88F, 0.84F, 0.92F, 0.88F, 0.97F, 1.1F, 0.94F, 0.74F, 0.71F, 0.74F, 0.72F, 0.7F, 0.73F, 0.72F, 0.76F, 0.82F, 0.77F, 0.77F, 0.73F, 0.74F, 0.71F, 0.7F, 0.73F, 0.83F, 0.85F, 0.78F, 0.92F, 0.88F, 0.86F, 0.81F, 0.79F, 0.74F, 0.75F, 0.92F, 0.93F, 0.85F, 0.96F, 0.94F, 0.88F, 0.86F, 0.81F, 0.75F, 0.79F, 0.93F, 0.9F, 0.85F, 0.88F, 0.82F, 0.77F, 1.05F, 1.27F, 0.99F, 1.6F, 1.47F, 1.39F, 1.2F, 1.11F, 0.95F, 0.97F, 1.08F, 1.33F, 1.31F, 1.7F, 1.55F, 1.76F, 1.57F, 1.76F, 1.7F, 1.54F, 1.85F, 1.97F, 1.91F, 1.99F, 1.97F, 1.99F, 1.91F, 1.77F, 1.88F, 1.85F, 1.39F, 1.64F, 1.51F, 1.58F, 1.74F, 1.32F, 1.22F, 1.01F, 1.54F, 1.76F, 1.65F, 1.93F, 1.7F, 1.85F, 1.28F, 1.39F, 1.09F, 1.52F, 1.48F, 1.26F, 0.97F, 0.99F, 1.18F, 1F, 0.93F, 0.9F, 1.05F, 1.01F, 1.31F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F}, new[]{1.97F, 1.92F, 1.88F, 1.79F, 1.79F, 1.71F, 1.37F, 1.59F, 1.38F, 1.6F, 1.35F, 1.23F, 1.47F, 1.3F, 1.56F, 1.99F, 1.93F, 1.9F, 1.6F, 1.78F, 1.61F, 1.79F, 1.57F, 1.48F, 1.72F, 1.4F, 1.14F, 1.37F, 0.89F, 0.96F, 0.94F, 1.07F, 1F, 1.21F, 1.3F, 1.14F, 0.98F, 0.96F, 0.86F, 0.91F, 0.83F, 0.82F, 0.88F, 0.82F, 0.89F, 1.11F, 0.87F, 0.94F, 0.93F, 1.02F, 1.07F, 0.8F, 0.79F, 0.85F, 0.82F, 0.8F, 0.87F, 0.85F, 0.93F, 1.02F, 0.93F, 0.77F, 0.72F, 0.74F, 0.71F, 0.7F, 0.7F, 0.71F, 0.72F, 0.77F, 0.74F, 0.82F, 0.76F, 0.79F, 0.72F, 0.73F, 0.76F, 0.89F, 0.89F, 0.82F, 0.93F, 0.91F, 0.86F, 0.83F, 0.79F, 0.73F, 0.76F, 0.91F, 0.89F, 0.83F, 0.89F, 0.89F, 0.82F, 0.82F, 0.76F, 0.72F, 0.76F, 0.86F, 0.83F, 0.79F, 0.82F, 0.76F, 0.73F, 0.94F, 1F, 0.91F, 1.37F, 1.21F, 1.14F, 0.98F, 0.96F, 0.88F, 0.89F, 0.96F, 1.14F, 1.07F, 1.6F, 1.4F, 1.61F, 1.37F, 1.57F, 1.48F, 1.3F, 1.78F, 1.93F, 1.79F, 1.99F, 1.92F, 1.9F, 1.79F, 1.59F, 1.72F, 1.79F, 1.3F, 1.56F, 1.35F, 1.38F, 1.6F, 1.11F, 1.07F, 0.94F, 1.68F, 1.86F, 1.71F, 1.97F, 1.68F, 1.86F, 1.44F, 1.49F, 1.22F, 1.44F, 1.49F, 1.22F, 0.99F, 0.99F, 1.23F, 1.19F, 0.98F, 0.97F, 0.97F, 0.98F, 1.19F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F}, new[]{1.94F, 1.97F, 1.87F, 1.91F, 1.85F, 1.71F, 1.6F, 1.77F, 1.58F, 1.74F, 1.51F, 1.26F, 1.48F, 1.39F, 1.64F, 1.99F, 1.97F, 1.99F, 1.7F, 1.85F, 1.76F, 1.91F, 1.76F, 1.7F, 1.88F, 1.55F, 1.33F, 1.57F, 0.96F, 1.08F, 1.05F, 1.31F, 1.27F, 1.47F, 1.54F, 1.39F, 1.2F, 1.11F, 0.93F, 0.99F, 0.9F, 0.88F, 0.95F, 0.88F, 0.97F, 1.32F, 0.92F, 1.01F, 0.97F, 1.1F, 1.22F, 0.84F, 0.8F, 0.88F, 0.79F, 0.79F, 0.85F, 0.86F, 0.92F, 1.02F, 0.94F, 0.82F, 0.76F, 0.77F, 0.72F, 0.73F, 0.7F, 0.72F, 0.71F, 0.74F, 0.74F, 0.88F, 0.81F, 0.85F, 0.75F, 0.77F, 0.82F, 0.94F, 0.93F, 0.86F, 0.92F, 0.92F, 0.86F, 0.85F, 0.79F, 0.74F, 0.79F, 0.88F, 0.85F, 0.81F, 0.82F, 0.83F, 0.77F, 0.78F, 0.73F, 0.71F, 0.75F, 0.79F, 0.77F, 0.74F, 0.77F, 0.73F, 0.7F, 0.86F, 0.92F, 0.84F, 1.14F, 0.99F, 0.98F, 0.91F, 0.9F, 0.84F, 0.83F, 0.88F, 0.97F, 0.94F, 1.41F, 1.18F, 1.39F, 1.11F, 1.33F, 1.24F, 1.03F, 1.61F, 1.8F, 1.59F, 1.91F, 1.84F, 1.76F, 1.64F, 1.38F, 1.51F, 1.71F, 1.26F, 1.5F, 1.23F, 1.19F, 1.46F, 0.99F, 1F, 0.91F, 1.7F, 1.85F, 1.65F, 1.93F, 1.54F, 1.76F, 1.52F, 1.48F, 1.26F, 1.28F, 1.39F, 1.09F, 0.99F, 0.97F, 1.18F, 1.31F, 1.01F, 1.05F, 0.9F, 0.93F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F}, new[]{1.86F, 1.95F, 1.82F, 1.98F, 1.89F, 1.71F, 1.8F, 1.91F, 1.77F, 1.86F, 1.67F, 1.34F, 1.53F, 1.51F, 1.72F, 1.92F, 1.91F, 1.99F, 1.69F, 1.82F, 1.8F, 1.94F, 1.87F, 1.86F, 1.97F, 1.59F, 1.44F, 1.69F, 1.05F, 1.24F, 1.27F, 1.49F, 1.5F, 1.69F, 1.72F, 1.63F, 1.46F, 1.37F, 1F, 1.23F, 0.98F, 0.95F, 1.09F, 0.96F, 1.16F, 1.55F, 0.99F, 1.25F, 1.1F, 1.24F, 1.41F, 0.9F, 0.85F, 0.94F, 0.79F, 0.81F, 0.85F, 0.89F, 0.94F, 1.09F, 0.98F, 0.89F, 0.82F, 0.83F, 0.74F, 0.77F, 0.72F, 0.76F, 0.73F, 0.75F, 0.78F, 0.94F, 0.86F, 0.91F, 0.79F, 0.83F, 0.89F, 0.99F, 0.95F, 0.9F, 0.9F, 0.92F, 0.84F, 0.86F, 0.79F, 0.75F, 0.81F, 0.85F, 0.8F, 0.78F, 0.76F, 0.77F, 0.73F, 0.74F, 0.71F, 0.71F, 0.73F, 0.74F, 0.74F, 0.71F, 0.76F, 0.72F, 0.7F, 0.79F, 0.85F, 0.78F, 0.98F, 0.92F, 0.93F, 0.85F, 0.87F, 0.82F, 0.79F, 0.81F, 0.89F, 0.86F, 1.16F, 0.97F, 1.12F, 0.95F, 1.06F, 1F, 0.93F, 1.38F, 1.6F, 1.35F, 1.77F, 1.71F, 1.57F, 1.48F, 1.2F, 1.28F, 1.62F, 1.27F, 1.46F, 1.17F, 1.05F, 1.34F, 0.96F, 0.99F, 0.9F, 1.63F, 1.74F, 1.5F, 1.8F, 1.33F, 1.58F, 1.48F, 1.37F, 1.21F, 1.04F, 1.21F, 0.97F, 0.97F, 0.93F, 1.05F, 1.34F, 1.02F, 1.14F, 0.84F, 0.88F, 0.92F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F}, new[]{1.74F, 1.89F, 1.76F, 1.98F, 1.89F, 1.71F, 1.93F, 1.99F, 1.91F, 1.94F, 1.82F, 1.46F, 1.6F, 1.65F, 1.8F, 1.79F, 1.77F, 1.92F, 1.57F, 1.69F, 1.74F, 1.87F, 1.88F, 1.94F, 1.98F, 1.53F, 1.45F, 1.7F, 1.18F, 1.32F, 1.42F, 1.58F, 1.65F, 1.83F, 1.81F, 1.81F, 1.67F, 1.61F, 1.19F, 1.44F, 1.17F, 1.11F, 1.36F, 1.15F, 1.41F, 1.75F, 1.22F, 1.5F, 1.34F, 1.42F, 1.61F, 0.98F, 0.92F, 1.03F, 0.83F, 0.86F, 0.89F, 0.95F, 0.98F, 1.23F, 1.14F, 0.97F, 0.89F, 0.9F, 0.78F, 0.82F, 0.76F, 0.82F, 0.77F, 0.79F, 0.84F, 0.98F, 0.9F, 0.98F, 0.83F, 0.89F, 0.97F, 1.03F, 0.95F, 0.92F, 0.86F, 0.9F, 0.82F, 0.86F, 0.79F, 0.77F, 0.84F, 0.81F, 0.76F, 0.76F, 0.72F, 0.73F, 0.7F, 0.72F, 0.71F, 0.73F, 0.73F, 0.72F, 0.74F, 0.71F, 0.78F, 0.74F, 0.72F, 0.75F, 0.8F, 0.76F, 0.94F, 0.88F, 0.91F, 0.83F, 0.87F, 0.84F, 0.79F, 0.76F, 0.82F, 0.8F, 0.97F, 0.89F, 0.96F, 0.88F, 0.95F, 0.94F, 0.87F, 1.11F, 1.37F, 1.1F, 1.59F, 1.57F, 1.37F, 1.33F, 1.05F, 1.08F, 1.54F, 1.34F, 1.46F, 1.16F, 0.99F, 1.26F, 0.96F, 1.05F, 0.92F, 1.45F, 1.55F, 1.27F, 1.6F, 1.07F, 1.34F, 1.35F, 1.18F, 1.07F, 0.93F, 0.99F, 0.9F, 0.93F, 0.87F, 0.96F, 1.27F, 0.99F, 1.15F, 0.77F, 0.82F, 0.85F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F}, new[]{1.6F, 1.78F, 1.68F, 1.93F, 1.86F, 1.71F, 1.97F, 1.99F, 1.99F, 1.97F, 1.93F, 1.6F, 1.68F, 1.78F, 1.86F, 1.61F, 1.57F, 1.79F, 1.37F, 1.48F, 1.59F, 1.72F, 1.79F, 1.92F, 1.9F, 1.38F, 1.35F, 1.6F, 1.23F, 1.3F, 1.47F, 1.56F, 1.71F, 1.88F, 1.79F, 1.92F, 1.79F, 1.79F, 1.3F, 1.56F, 1.35F, 1.37F, 1.59F, 1.38F, 1.6F, 1.9F, 1.48F, 1.72F, 1.57F, 1.61F, 1.79F, 1.21F, 1F, 1.3F, 0.89F, 0.94F, 0.96F, 1.07F, 1.14F, 1.4F, 1.37F, 1.14F, 0.96F, 0.98F, 0.82F, 0.88F, 0.82F, 0.89F, 0.83F, 0.86F, 0.91F, 1.02F, 0.93F, 1.07F, 0.87F, 0.94F, 1.11F, 1.02F, 0.93F, 0.93F, 0.82F, 0.87F, 0.8F, 0.85F, 0.79F, 0.8F, 0.85F, 0.77F, 0.72F, 0.74F, 0.71F, 0.7F, 0.7F, 0.71F, 0.72F, 0.77F, 0.74F, 0.72F, 0.76F, 0.73F, 0.82F, 0.79F, 0.76F, 0.73F, 0.79F, 0.76F, 0.93F, 0.86F, 0.91F, 0.83F, 0.89F, 0.89F, 0.82F, 0.72F, 0.76F, 0.76F, 0.89F, 0.82F, 0.89F, 0.82F, 0.89F, 0.91F, 0.83F, 0.96F, 1.14F, 0.97F, 1.4F, 1.44F, 1.19F, 1.22F, 0.99F, 0.98F, 1.49F, 1.44F, 1.49F, 1.22F, 0.99F, 1.23F, 0.98F, 1.19F, 0.97F, 1.21F, 1.3F, 1F, 1.37F, 0.94F, 1.07F, 1.14F, 0.98F, 0.96F, 0.86F, 0.91F, 0.83F, 0.88F, 0.82F, 0.89F, 1.11F, 0.94F, 1.07F, 0.73F, 0.76F, 0.79F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F}, new[]{1.46F, 1.65F, 1.6F, 1.82F, 1.8F, 1.71F, 1.93F, 1.91F, 1.99F, 1.94F, 1.98F, 1.74F, 1.76F, 1.89F, 1.89F, 1.42F, 1.34F, 1.61F, 1.11F, 1.22F, 1.36F, 1.5F, 1.61F, 1.81F, 1.75F, 1.15F, 1.17F, 1.41F, 1.18F, 1.19F, 1.42F, 1.44F, 1.65F, 1.83F, 1.67F, 1.94F, 1.81F, 1.88F, 1.32F, 1.58F, 1.45F, 1.57F, 1.74F, 1.53F, 1.7F, 1.98F, 1.69F, 1.87F, 1.77F, 1.79F, 1.92F, 1.45F, 1.27F, 1.55F, 0.97F, 1.07F, 1.11F, 1.34F, 1.37F, 1.59F, 1.6F, 1.35F, 1.07F, 1.18F, 0.86F, 0.93F, 0.87F, 0.96F, 0.9F, 0.93F, 0.99F, 1.03F, 0.95F, 1.15F, 0.9F, 0.99F, 1.27F, 0.98F, 0.9F, 0.92F, 0.78F, 0.83F, 0.77F, 0.84F, 0.79F, 0.82F, 0.86F, 0.73F, 0.71F, 0.73F, 0.72F, 0.7F, 0.73F, 0.72F, 0.76F, 0.81F, 0.76F, 0.76F, 0.82F, 0.77F, 0.89F, 0.85F, 0.82F, 0.75F, 0.8F, 0.8F, 0.94F, 0.88F, 0.94F, 0.87F, 0.95F, 0.96F, 0.88F, 0.72F, 0.74F, 0.76F, 0.83F, 0.78F, 0.84F, 0.79F, 0.87F, 0.91F, 0.83F, 0.89F, 0.98F, 0.92F, 1.23F, 1.34F, 1.05F, 1.16F, 0.99F, 0.96F, 1.46F, 1.57F, 1.54F, 1.33F, 1.05F, 1.26F, 1.08F, 1.37F, 1.1F, 0.98F, 1.03F, 0.92F, 1.14F, 0.86F, 0.95F, 0.97F, 0.9F, 0.89F, 0.79F, 0.84F, 0.77F, 0.82F, 0.76F, 0.82F, 0.97F, 0.89F, 0.98F, 0.71F, 0.72F, 0.74F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F}, new[]{1.34F, 1.51F, 1.53F, 1.67F, 1.72F, 1.71F, 1.8F, 1.77F, 1.91F, 1.86F, 1.98F, 1.86F, 1.82F, 1.95F, 1.89F, 1.24F, 1.1F, 1.41F, 0.95F, 0.99F, 1.09F, 1.25F, 1.37F, 1.63F, 1.55F, 0.96F, 0.98F, 1.16F, 1.05F, 1F, 1.27F, 1.23F, 1.5F, 1.69F, 1.46F, 1.86F, 1.72F, 1.87F, 1.24F, 1.49F, 1.44F, 1.69F, 1.8F, 1.59F, 1.69F, 1.97F, 1.82F, 1.94F, 1.91F, 1.92F, 1.99F, 1.63F, 1.5F, 1.74F, 1.16F, 1.33F, 1.38F, 1.58F, 1.6F, 1.77F, 1.8F, 1.48F, 1.21F, 1.37F, 0.9F, 0.97F, 0.93F, 1.05F, 0.97F, 1.04F, 1.21F, 0.99F, 0.95F, 1.14F, 0.92F, 1.02F, 1.34F, 0.94F, 0.86F, 0.9F, 0.74F, 0.79F, 0.75F, 0.81F, 0.79F, 0.84F, 0.86F, 0.71F, 0.71F, 0.73F, 0.76F, 0.73F, 0.77F, 0.74F, 0.8F, 0.85F, 0.78F, 0.81F, 0.89F, 0.84F, 0.97F, 0.92F, 0.88F, 0.79F, 0.85F, 0.86F, 0.98F, 0.92F, 1F, 0.93F, 1.06F, 1.12F, 0.95F, 0.74F, 0.74F, 0.78F, 0.79F, 0.76F, 0.82F, 0.79F, 0.87F, 0.93F, 0.85F, 0.85F, 0.94F, 0.9F, 1.09F, 1.27F, 0.99F, 1.17F, 1.05F, 0.96F, 1.46F, 1.71F, 1.62F, 1.48F, 1.2F, 1.34F, 1.28F, 1.57F, 1.35F, 0.9F, 0.94F, 0.85F, 0.98F, 0.81F, 0.89F, 0.89F, 0.83F, 0.82F, 0.75F, 0.78F, 0.73F, 0.77F, 0.72F, 0.76F, 0.89F, 0.83F, 0.91F, 0.71F, 0.7F, 0.72F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F}, new[]{1.26F, 1.39F, 1.48F, 1.51F, 1.64F, 1.71F, 1.6F, 1.58F, 1.77F, 1.74F, 1.91F, 1.94F, 1.87F, 1.97F, 1.85F, 1.1F, 0.97F, 1.22F, 0.88F, 0.92F, 0.95F, 1.01F, 1.11F, 1.39F, 1.32F, 0.88F, 0.9F, 0.97F, 0.96F, 0.93F, 1.05F, 0.99F, 1.27F, 1.47F, 1.2F, 1.7F, 1.54F, 1.76F, 1.08F, 1.31F, 1.33F, 1.7F, 1.76F, 1.55F, 1.57F, 1.88F, 1.85F, 1.91F, 1.97F, 1.99F, 1.99F, 1.7F, 1.65F, 1.85F, 1.41F, 1.54F, 1.61F, 1.76F, 1.8F, 1.91F, 1.93F, 1.52F, 1.26F, 1.48F, 0.92F, 0.99F, 0.97F, 1.18F, 1.09F, 1.28F, 1.39F, 0.94F, 0.93F, 1.05F, 0.92F, 1.01F, 1.31F, 0.88F, 0.81F, 0.86F, 0.72F, 0.75F, 0.74F, 0.79F, 0.79F, 0.86F, 0.85F, 0.71F, 0.73F, 0.75F, 0.82F, 0.77F, 0.83F, 0.78F, 0.85F, 0.88F, 0.81F, 0.88F, 0.97F, 0.9F, 1.18F, 1F, 0.93F, 0.86F, 0.92F, 0.94F, 1.14F, 0.99F, 1.24F, 1.03F, 1.33F, 1.39F, 1.11F, 0.79F, 0.77F, 0.84F, 0.79F, 0.77F, 0.84F, 0.83F, 0.9F, 0.98F, 0.91F, 0.85F, 0.92F, 0.91F, 1.02F, 1.26F, 1F, 1.23F, 1.19F, 0.99F, 1.5F, 1.84F, 1.71F, 1.64F, 1.38F, 1.46F, 1.51F, 1.76F, 1.59F, 0.84F, 0.88F, 0.8F, 0.94F, 0.79F, 0.86F, 0.82F, 0.77F, 0.76F, 0.74F, 0.74F, 0.71F, 0.73F, 0.7F, 0.72F, 0.82F, 0.77F, 0.85F, 0.74F, 0.7F, 0.73F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F, 1F}};
    }
}