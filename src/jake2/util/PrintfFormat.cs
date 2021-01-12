using J2N.Text;
using System;
using System.Collections;
using System.Globalization;

namespace Jake2.Util
{
	public class PrintfFormat
	{
		public PrintfFormat( String fmtArg )
		{
			var ePos = 0;
			ConversionSpecification sFmt = null;
			var unCS = this.NonControl( fmtArg, 0 );
			if ( unCS != null )
			{
				sFmt = new ConversionSpecification();
				sFmt.SetLiteral( unCS );
				vFmt.Add( sFmt );
			}

			while ( cPos != -1 && cPos < fmtArg.Length )
			{
				for ( ePos = cPos + 1; ePos < fmtArg.Length; ePos++ )
				{
					var c = fmtArg[ePos];
					if ( c == 'i' )
						break;
					if ( c == 'd' )
						break;
					if ( c == 'f' )
						break;
					if ( c == 'g' )
						break;
					if ( c == 'G' )
						break;
					if ( c == 'o' )
						break;
					if ( c == 'x' )
						break;
					if ( c == 'X' )
						break;
					if ( c == 'e' )
						break;
					if ( c == 'E' )
						break;
					if ( c == 'c' )
						break;
					if ( c == 's' )
						break;
					if ( c == '%' )
						break;
				}

				ePos = Math.Min( ePos + 1, fmtArg.Length );
				sFmt = new ConversionSpecification( fmtArg.Substring( cPos, ePos ) );
				vFmt.Add( sFmt );
				unCS = this.NonControl( fmtArg, ePos );
				if ( unCS != null )
				{
					sFmt = new ConversionSpecification();
					sFmt.SetLiteral( unCS );
					vFmt.Add( sFmt );
				}
			}
		}

		private String NonControl( String s, Int32 start )
		{
			var ret = "";
			cPos = s.IndexOf( "%", start );
			if ( cPos == -1 )
				cPos = s.Length;
			return s.Substring( start, cPos );
		}

		public virtual String Sprintf( Object[] o )
		{
			var e = vFmt.GetEnumerator();
			ConversionSpecification cs = null;
			var c = ( Char ) 0;
			var i = 0;
			StringBuffer sb = new StringBuffer();
			while ( e.MoveNext() )
			{
				cs = ( ConversionSpecification ) e.Current;
				c = cs.GetConversionCharacter();
				if ( c == '\\' )
					sb.Append( cs.GetLiteral() );
				else if ( c == '%' )
					sb.Append( "%" );
				else
				{
					if ( cs.IsPositionalSpecification() )
					{
						i = cs.GetArgumentPosition() - 1;
						if ( cs.IsPositionalFieldWidth() )
						{
							var ifw = cs.GetArgumentPositionForFieldWidth() - 1;
							cs.SetFieldWidthWithArg( ( ( Int32 ) o[ifw] ) );
						}

						if ( cs.IsPositionalPrecision() )
						{
							var ipr = cs.GetArgumentPositionForPrecision() - 1;
							cs.SetPrecisionWithArg( ( ( Int32 ) o[ipr] ) );
						}
					}
					else
					{
						if ( cs.IsVariableFieldWidth() )
						{
							cs.SetFieldWidthWithArg( ( ( Int32 ) o[i] ) );
							i++;
						}

						if ( cs.IsVariablePrecision() )
						{
							cs.SetPrecisionWithArg( ( ( Int32 ) o[i] ) );
							i++;
						}
					}

					if ( o[i] is Byte )
						sb.Append( cs.Internalsprintf( ( ( Byte ) o[i] ) ) );
					else if ( o[i] is Int16 )
						sb.Append( cs.Internalsprintf( ( ( Int16 ) o[i] ) ) );
					else if ( o[i] is Int32 )
						sb.Append( cs.Internalsprintf( ( ( Int32 ) o[i] ) ) );
					else if ( o[i] is Int64 )
						sb.Append( cs.Internalsprintf( ( ( Int64 ) o[i] ) ) );
					else if ( o[i] is Single )
						sb.Append( cs.Internalsprintf( ( ( Single ) o[i] ) ) );
					else if ( o[i] is Double )
						sb.Append( cs.Internalsprintf( ( ( Double ) o[i] ) ) );
					else if ( o[i] is Char )
						sb.Append( cs.Internalsprintf( ( ( Char ) o[i] ) ) );
					else if ( o[i] is String )
						sb.Append( cs.Internalsprintf( ( String ) o[i] ) );
					else
						sb.Append( cs.Internalsprintf( o[i] ) );
					if ( !cs.IsPositionalSpecification() )
						i++;
				}
			}

			return sb.ToString();
		}

		public virtual String Sprintf( Object aasd, params Object[] parameters )
		{
			var c = ( Char ) 0;
			StringBuffer sb = new StringBuffer();
			foreach ( ConversionSpecification cs in vFmt )
			{
				c = cs.GetConversionCharacter();
				if ( c == '\\' )
					sb.Append( cs.GetLiteral() );
				else if ( c == '%' )
					sb.Append( "%" );
			}

			return sb.ToString();
		}

		public virtual String Sprintf( Int32 x, params Object[] parameters )
		{
			var c = ( Char ) 0;
			StringBuffer sb = new StringBuffer();
			foreach ( ConversionSpecification cs in vFmt )
			{
				c = cs.GetConversionCharacter();
				if ( c == '\\' )
					sb.Append( cs.GetLiteral() );
				else if ( c == '%' )
					sb.Append( "%" );
				else
					sb.Append( cs.Internalsprintf( x ) );
			}

			return sb.ToString();
		}

		public virtual String Sprintf( Int64 x )
		{
			var c = ( Char ) 0;
			StringBuffer sb = new StringBuffer();
			foreach ( ConversionSpecification cs in vFmt )
			{
				c = cs.GetConversionCharacter();
				if ( c == '\\' )
					sb.Append( cs.GetLiteral() );
				else if ( c == '%' )
					sb.Append( "%" );
				else
					sb.Append( cs.Internalsprintf( x ) );
			}

			return sb.ToString();
		}

		public virtual String Sprintf( Double x )
		{
			var c = ( Char ) 0;
			StringBuffer sb = new StringBuffer();
			foreach ( ConversionSpecification cs in vFmt )
			{
				c = cs.GetConversionCharacter();
				if ( c == '\\' )
					sb.Append( cs.GetLiteral() );
				else if ( c == '%' )
					sb.Append( "%" );
				else
					sb.Append( cs.Internalsprintf( x ) );
			}

			return sb.ToString();
		}

		public virtual String Sprintf( String x, params Object[] parameters )
		{
			var c = ( Char ) 0;
			StringBuffer sb = new StringBuffer();
			foreach ( ConversionSpecification cs in vFmt )
			{
				c = cs.GetConversionCharacter();
				if ( c == '\\' )
					sb.Append( cs.GetLiteral() );
				else if ( c == '%' )
					sb.Append( "%" );
				else
					sb.Append( cs.Internalsprintf( x ) );
			}

			return sb.ToString();
		}

		public virtual String Sprintf( Object x )
		{
			var c = ( Char ) 0;
			StringBuffer sb = new StringBuffer();
			foreach ( ConversionSpecification cs in vFmt )
			{
				c = cs.GetConversionCharacter();
				if ( c == '\\' )
					sb.Append( cs.GetLiteral() );
				else if ( c == '%' )
					sb.Append( "%" );
				else
				{
					if ( x is Byte )
						sb.Append( cs.Internalsprintf( ( ( Byte ) x ) ) );
					else if ( x is Int16 )
						sb.Append( cs.Internalsprintf( ( ( Int16 ) x ) ) );
					else if ( x is Int32 )
						sb.Append( cs.Internalsprintf( ( ( Int32 ) x ) ) );
					else if ( x is Int64 )
						sb.Append( cs.Internalsprintf( ( ( Int64 ) x ) ) );
					else if ( x is Single )
						sb.Append( cs.Internalsprintf( ( ( Single ) x ) ) );
					else if ( x is Double )
						sb.Append( cs.Internalsprintf( ( ( Double ) x ) ) );
					else if ( x is Char )
						sb.Append( cs.Internalsprintf( ( ( Char ) x ) ) );
					else if ( x is String )
						sb.Append( cs.Internalsprintf( ( String ) x ) );
					else
						sb.Append( cs.Internalsprintf( x ) );
				}
			}

			return sb.ToString();
		}

		public class ConversionSpecification
		{
			public ConversionSpecification( )
			{
			}

			public ConversionSpecification( String fmtArg )
			{
				if ( fmtArg == null )
					throw new NullReferenceException();
				if ( fmtArg.Length == 0 )
					throw new ArgumentException( "Control strings must have positive" + " lengths." );
				if ( fmtArg[0] == '%' )
				{
					fmt = fmtArg;
					pos = 1;
					SetArgPosition();
					SetFlagCharacters();
					SetFieldWidth();
					SetPrecision();
					SetOptionalHL();
					if ( SetConversionCharacter() )
					{
						if ( pos == fmtArg.Length )
						{
							if ( leadingZeros && leftJustify )
								leadingZeros = false;
							if ( precisionSet && leadingZeros )
							{
								if ( conversionCharacter == 'd' || conversionCharacter == 'i' || conversionCharacter == 'o' || conversionCharacter == 'x' )
								{
									leadingZeros = false;
								}
							}
						}
						else
							throw new ArgumentException( "Malformed conversion specification=" + fmtArg );
					}
					else
						throw new ArgumentException( "Malformed conversion specification=" + fmtArg );
				}
				else
					throw new ArgumentException( "Control strings must begin with %." );
			}

			public virtual void SetLiteral( String s )
			{
				fmt = s;
			}

			public virtual String GetLiteral( )
			{
				StringBuffer sb = new StringBuffer();
				var i = 0;
				while ( i < fmt.Length )
				{
					if ( fmt[i] == '\\' )
					{
						i++;
						if ( i < fmt.Length )
						{
							var c = fmt[i];
							switch ( c )
							{
								case 'a':
									sb.Append( ( Char ) 0x07 );
									break;
								case 'b':
									sb.Append( '\\' );
									break;
								case 'f':
									sb.Append( '\\' );
									break;
								case 'n':
									sb.Append( Environment.NewLine );
									break;
								case 'r':
									sb.Append( '\\' );
									break;
								case 't':
									sb.Append( '\\' );
									break;
								case 'v':
									sb.Append( ( Char ) 0x0b );
									break;
								case '\\':
									sb.Append( '\\' );
									break;
							}

							i++;
						}
						else
							sb.Append( '\\' );
					}
					else
						i++;
				}

				return fmt;
			}

			public virtual Char GetConversionCharacter( )
			{
				return conversionCharacter;
			}

			public virtual Boolean IsVariableFieldWidth( )
			{
				return variableFieldWidth;
			}

			public virtual void SetFieldWidthWithArg( Int32 fw )
			{
				if ( fw < 0 )
					leftJustify = true;
				fieldWidthSet = true;
				fieldWidth = Math.Abs( fw );
			}

			public virtual Boolean IsVariablePrecision( )
			{
				return variablePrecision;
			}

			public virtual void SetPrecisionWithArg( Int32 pr )
			{
				precisionSet = true;
				precision = Math.Max( pr, 0 );
			}

			public String Internalsprintf( Int32 s )
			{
				var s2 = "";
				switch ( conversionCharacter )
				{
					case 'd':
					case 'i':
						if ( optionalh )
							s2 = PrintDFormat( ( Int16 ) s );
						else if ( optionall )
							s2 = PrintDFormat( ( Int64 ) s );
						else
							s2 = PrintDFormat( s );
						break;
					case 'x':
					case 'X':
						if ( optionalh )
							s2 = PrintXFormat( ( Int16 ) s );
						else if ( optionall )
							s2 = PrintXFormat( ( Int64 ) s );
						else
							s2 = PrintXFormat( s );
						break;
					case 'o':
						if ( optionalh )
							s2 = PrintOFormat( ( Int16 ) s );
						else if ( optionall )
							s2 = PrintOFormat( ( Int64 ) s );
						else
							s2 = PrintOFormat( s );
						break;
					case 'c':
					case 'C':
						s2 = PrintCFormat( ( Char ) s );
						break;
					default:
						throw new ArgumentException( "Cannot format a int with a format using a " + conversionCharacter + " conversion character." );
				}

				return s2;
			}

			public virtual String Internalsprintf( Int64 s )
			{
				var s2 = "";
				switch ( conversionCharacter )
				{
					case 'd':
					case 'i':
						if ( optionalh )
							s2 = PrintDFormat( ( Int16 ) s );
						else if ( optionall )
							s2 = PrintDFormat( s );
						else
							s2 = PrintDFormat( ( Int32 ) s );
						break;
					case 'x':
					case 'X':
						if ( optionalh )
							s2 = PrintXFormat( ( Int16 ) s );
						else if ( optionall )
							s2 = PrintXFormat( s );
						else
							s2 = PrintXFormat( ( Int32 ) s );
						break;
					case 'o':
						if ( optionalh )
							s2 = PrintOFormat( ( Int16 ) s );
						else if ( optionall )
							s2 = PrintOFormat( s );
						else
							s2 = PrintOFormat( ( Int32 ) s );
						break;
					case 'c':
					case 'C':
						s2 = PrintCFormat( ( Char ) s );
						break;
					default:
						throw new ArgumentException( "Cannot format a long with a format using a " + conversionCharacter + " conversion character." );
				}

				return s2;
			}

			public virtual String Internalsprintf( Double s )
			{
				var s2 = "";
				switch ( conversionCharacter )
				{
					case 'f':
						s2 = PrintFFormat( s );
						break;
					case 'E':
					case 'e':
						s2 = PrintEFormat( s );
						break;
					case 'G':
					case 'g':
						s2 = PrintGFormat( s );
						break;
					default:
						throw new ArgumentException( "Cannot " + "format a double with a format using a " + conversionCharacter + " conversion character." );
				}

				return s2;
			}

			public virtual String Internalsprintf( String s )
			{
				var s2 = "";
				if ( conversionCharacter == 's' || conversionCharacter == 'S' )
					s2 = PrintSFormat( s );
				else
					throw new ArgumentException( "Cannot " + "format a String with a format using a " + conversionCharacter + " conversion character." );
				return s2;
			}

			public virtual String Internalsprintf( Object s )
			{
				var s2 = "";
				if ( conversionCharacter == 's' || conversionCharacter == 'S' )
					s2 = PrintSFormat( s.ToString() );
				else
					throw new ArgumentException( "Cannot format a String with a format using" + " a " + conversionCharacter + " conversion character." );
				return s2;
			}

			private Char[] FFormatDigits( Double x )
			{
				String sx, sxOut;
				Int32 i, j, k;
				Int32 n1In, n2In;
				var expon = 0;
				var minusSign = false;
				if ( x > 0 )
					sx = x.ToString();
				else if ( x < 0 )
				{
					sx = ( -x ).ToString();
					minusSign = true;
				}
				else
				{
					sx = x.ToString();
					if ( sx[0] == '-' )
					{
						minusSign = true;
						sx = sx.Substring( 1 );
					}
				}

				var ePos = sx.IndexOf( 'E' );
				var rPos = sx.IndexOf( '.' );
				if ( rPos != -1 )
					n1In = rPos;
				else if ( ePos != -1 )
					n1In = ePos;
				else
					n1In = sx.Length;
				if ( rPos != -1 )
				{
					if ( ePos != -1 )
						n2In = ePos - rPos - 1;
					else
						n2In = sx.Length - rPos - 1;
				}
				else
					n2In = 0;
				if ( ePos != -1 )
				{
					var ie = ePos + 1;
					expon = 0;
					if ( sx[ie] == '-' )
					{
						for ( ++ie; ie < sx.Length; ie++ )
							if ( sx[ie] != '0' )
								break;
						if ( ie < sx.Length )
							expon = -Int32.Parse( sx.Substring( ie ) );
					}
					else
					{
						if ( sx[ie] == '+' )
							++ie;
						for ( ; ie < sx.Length; ie++ )
							if ( sx[ie] != '0' )
								break;
						if ( ie < sx.Length )
							expon = Int32.Parse( sx.Substring( ie ) );
					}
				}

				Int32 p;
				if ( precisionSet )
					p = precision;
				else
					p = defaultDigits - 1;
				Char[] ca1 = sx.ToCharArray();
				Char[] ca2 = new Char[n1In + n2In];
				Char[] ca3, ca4, ca5;
				for ( j = 0; j < n1In; j++ )
					ca2[j] = ca1[j];
				i = j + 1;
				for ( k = 0; k < n2In; j++, i++, k++ )
					ca2[j] = ca1[i];
				if ( n1In + expon <= 0 )
				{
					ca3 = new Char[-expon + n2In];
					for ( j = 0, k = 0; k < ( -n1In - expon ); k++, j++ )
						ca3[j] = '0';
					for ( i = 0; i < ( n1In + n2In ); i++, j++ )
						ca3[j] = ca2[i];
				}
				else
					ca3 = ca2;
				var carry = false;
				if ( p < -expon + n2In )
				{
					if ( expon < 0 )
						i = p;
					else
						i = p + n1In;
					carry = CheckForCarry( ca3, i );
					if ( carry )
						carry = StartSymbolicCarry( ca3, i - 1, 0 );
				}

				if ( n1In + expon <= 0 )
				{
					ca4 = new Char[2 + p];
					if ( !carry )
						ca4[0] = '0';
					else
						ca4[0] = '1';
					if ( alternateForm || !precisionSet || precision != 0 )
					{
						ca4[1] = '.';
						for ( i = 0, j = 2; i < Math.Min( p, ca3.Length ); i++, j++ )
							ca4[j] = ca3[i];
						for ( ; j < ca4.Length; j++ )
							ca4[j] = '0';
					}
				}
				else
				{
					if ( !carry )
					{
						if ( alternateForm || !precisionSet || precision != 0 )
							ca4 = new Char[n1In + expon + p + 1];
						else
							ca4 = new Char[n1In + expon];
						j = 0;
					}
					else
					{
						if ( alternateForm || !precisionSet || precision != 0 )
							ca4 = new Char[n1In + expon + p + 2];
						else
							ca4 = new Char[n1In + expon + 1];
						ca4[0] = '1';
						j = 1;
					}

					for ( i = 0; i < Math.Min( n1In + expon, ca3.Length ); i++, j++ )
						ca4[j] = ca3[i];
					for ( ; i < n1In + expon; i++, j++ )
						ca4[j] = '0';
					if ( alternateForm || !precisionSet || precision != 0 )
					{
						ca4[j] = '.';
						j++;
						for ( k = 0; i < ca3.Length && k < p; i++, j++, k++ )
							ca4[j] = ca3[i];
						for ( ; j < ca4.Length; j++ )
							ca4[j] = '0';
					}
				}

				var nZeros = 0;
				if ( !leftJustify && leadingZeros )
				{
					var xThousands = 0;
					if ( thousands )
					{
						var xlead = 0;
						if ( ca4[0] == '+' || ca4[0] == '-' || ca4[0] == ' ' )
							xlead = 1;
						var xdp = xlead;
						for ( ; xdp < ca4.Length; xdp++ )
							if ( ca4[xdp] == '.' )
								break;
						xThousands = ( xdp - xlead ) / 3;
					}

					if ( fieldWidthSet )
						nZeros = fieldWidth - ca4.Length;
					if ( ( !minusSign && ( leadingSign || leadingSpace ) ) || minusSign )
						nZeros--;
					nZeros -= xThousands;
					if ( nZeros < 0 )
						nZeros = 0;
				}

				j = 0;
				if ( ( !minusSign && ( leadingSign || leadingSpace ) ) || minusSign )
				{
					ca5 = new Char[ca4.Length + nZeros + 1];
					j++;
				}
				else
					ca5 = new Char[ca4.Length + nZeros];
				if ( !minusSign )
				{
					if ( leadingSign )
						ca5[0] = '+';
					if ( leadingSpace )
						ca5[0] = ' ';
				}
				else
					ca5[0] = '-';
				for ( i = 0; i < nZeros; i++, j++ )
					ca5[j] = '0';
				for ( i = 0; i < ca4.Length; i++, j++ )
					ca5[j] = ca4[i];
				var lead = 0;
				if ( ca5[0] == '+' || ca5[0] == '-' || ca5[0] == ' ' )
					lead = 1;
				var dp = lead;
				for ( ; dp < ca5.Length; dp++ )
					if ( ca5[dp] == '.' )
						break;
				var nThousands = ( dp - lead ) / 3;
				if ( dp < ca5.Length )
					ca5[dp] = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0];
				Char[] ca6 = ca5;
				if ( thousands && nThousands > 0 )
				{
					ca6 = new Char[ca5.Length + nThousands + lead];
					ca6[0] = ca5[0];
					for ( i = lead, k = lead; i < dp; i++ )
					{
						if ( i > 0 && ( dp - i ) % 3 == 0 )
						{
							ca6[k] = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0];
							ca6[k + 1] = ca5[i];
							k += 2;
						}
						else
						{
							ca6[k] = ca5[i];
							k++;
						}
					}

					for ( ; i < ca5.Length; i++, k++ )
					{
						ca6[k] = ca5[i];
					}
				}

				return ca6;
			}

			private String FFormatString( Double x )
			{
				var noDigits = false;
				Char[] ca6, ca7;
				if ( Double.IsInfinity( x ) )
				{
					if ( x == Double.PositiveInfinity )
					{
						if ( leadingSign )
							ca6 = "+Inf".ToCharArray();
						else if ( leadingSpace )
							ca6 = " Inf".ToCharArray();
						else
							ca6 = "Inf".ToCharArray();
					}
					else
						ca6 = "-Inf".ToCharArray();
					noDigits = true;
				}
				else if ( Double.IsNaN( x ) )
				{
					if ( leadingSign )
						ca6 = "+NaN".ToCharArray();
					else if ( leadingSpace )
						ca6 = " NaN".ToCharArray();
					else
						ca6 = "NaN".ToCharArray();
					noDigits = true;
				}
				else
					ca6 = FFormatDigits( x );
				ca7 = ApplyFloatPadding( ca6, false );
				return new String( ca7 );
			}

			private Char[] EFormatDigits( Double x, Char eChar )
			{
				Char[] ca1, ca2, ca3;
				String sx, sxOut;
				Int32 i, j, k, p;
				Int32 n1In, n2In;
				var expon = 0;
				Int32 ePos, rPos, eSize;
				var minusSign = false;
				if ( x > 0 )
					sx = x.ToString();
				else if ( x < 0 )
				{
					sx = ( -x ).ToString();
					minusSign = true;
				}
				else
				{
					sx = x.ToString();
					if ( sx[0] == '-' )
					{
						minusSign = true;
						sx = sx.Substring( 1 );
					}
				}

				ePos = sx.IndexOf( 'E' );
				if ( ePos == -1 )
					ePos = sx.IndexOf( 'e' );
				rPos = sx.IndexOf( '.' );
				if ( rPos != -1 )
					n1In = rPos;
				else if ( ePos != -1 )
					n1In = ePos;
				else
					n1In = sx.Length;
				if ( rPos != -1 )
				{
					if ( ePos != -1 )
						n2In = ePos - rPos - 1;
					else
						n2In = sx.Length - rPos - 1;
				}
				else
					n2In = 0;
				if ( ePos != -1 )
				{
					var ie = ePos + 1;
					expon = 0;
					if ( sx[ie] == '-' )
					{
						for ( ++ie; ie < sx.Length; ie++ )
							if ( sx[ie] != '0' )
								break;
						if ( ie < sx.Length )
							expon = -Int32.Parse( sx.Substring( ie ) );
					}
					else
					{
						if ( sx[ie] == '+' )
							++ie;
						for ( ; ie < sx.Length; ie++ )
							if ( sx[ie] != '0' )
								break;
						if ( ie < sx.Length )
							expon = Int32.Parse( sx.Substring( ie ) );
					}
				}

				if ( rPos != -1 )
					expon += rPos - 1;
				if ( precisionSet )
					p = precision;
				else
					p = defaultDigits - 1;
				if ( rPos != -1 && ePos != -1 )
					ca1 = ( sx.Substring( 0, rPos ) + sx.Substring( rPos + 1, ePos ) ).ToCharArray();
				else if ( rPos != -1 )
					ca1 = ( sx.Substring( 0, rPos ) + sx.Substring( rPos + 1 ) ).ToCharArray();
				else if ( ePos != -1 )
					ca1 = sx.Substring( 0, ePos ).ToCharArray();
				else
					ca1 = sx.ToCharArray();
				var carry = false;
				var i0 = 0;
				if ( ca1[0] != '0' )
					i0 = 0;
				else
					for ( i0 = 0; i0 < ca1.Length; i0++ )
						if ( ca1[i0] != '0' )
							break;
				if ( i0 + p < ca1.Length - 1 )
				{
					carry = CheckForCarry( ca1, i0 + p + 1 );
					if ( carry )
						carry = StartSymbolicCarry( ca1, i0 + p, i0 );
					if ( carry )
					{
						ca2 = new Char[i0 + p + 1];
						ca2[i0] = '1';
						for ( j = 0; j < i0; j++ )
							ca2[j] = '0';
						for ( i = i0, j = i0 + 1; j < p + 1; i++, j++ )
							ca2[j] = ca1[i];
						expon++;
						ca1 = ca2;
					}
				}

				if ( Math.Abs( expon ) < 100 && !optionalL )
					eSize = 4;
				else
					eSize = 5;
				if ( alternateForm || !precisionSet || precision != 0 )
					ca2 = new Char[2 + p + eSize];
				else
					ca2 = new Char[1 + eSize];
				if ( ca1[0] != '0' )
				{
					ca2[0] = ca1[0];
					j = 1;
				}
				else
				{
					for ( j = 1; j < ( ePos == -1 ? ca1.Length : ePos ); j++ )
						if ( ca1[j] != '0' )
							break;
					if ( ( ePos != -1 && j < ePos ) || ( ePos == -1 && j < ca1.Length ) )
					{
						ca2[0] = ca1[j];
						expon -= j;
						j++;
					}
					else
					{
						ca2[0] = '0';
						j = 2;
					}
				}

				if ( alternateForm || !precisionSet || precision != 0 )
				{
					ca2[1] = '.';
					i = 2;
				}
				else
					i = 1;
				for ( k = 0; k < p && j < ca1.Length; j++, i++, k++ )
					ca2[i] = ca1[j];
				for ( ; i < ca2.Length - eSize; i++ )
					ca2[i] = '0';
				ca2[i++] = eChar;
				if ( expon < 0 )
					ca2[i++] = '-';
				else
					ca2[i++] = '+';
				expon = Math.Abs( expon );
				if ( expon >= 100 )
				{
					switch ( expon / 100 )
					{
						case 1:
							ca2[i] = '1';
							break;
						case 2:
							ca2[i] = '2';
							break;
						case 3:
							ca2[i] = '3';
							break;
						case 4:
							ca2[i] = '4';
							break;
						case 5:
							ca2[i] = '5';
							break;
						case 6:
							ca2[i] = '6';
							break;
						case 7:
							ca2[i] = '7';
							break;
						case 8:
							ca2[i] = '8';
							break;
						case 9:
							ca2[i] = '9';
							break;
					}

					i++;
				}

				switch ( ( expon % 100 ) / 10 )
				{
					case 0:
						ca2[i] = '0';
						break;
					case 1:
						ca2[i] = '1';
						break;
					case 2:
						ca2[i] = '2';
						break;
					case 3:
						ca2[i] = '3';
						break;
					case 4:
						ca2[i] = '4';
						break;
					case 5:
						ca2[i] = '5';
						break;
					case 6:
						ca2[i] = '6';
						break;
					case 7:
						ca2[i] = '7';
						break;
					case 8:
						ca2[i] = '8';
						break;
					case 9:
						ca2[i] = '9';
						break;
				}

				i++;
				switch ( expon % 10 )
				{
					case 0:
						ca2[i] = '0';
						break;
					case 1:
						ca2[i] = '1';
						break;
					case 2:
						ca2[i] = '2';
						break;
					case 3:
						ca2[i] = '3';
						break;
					case 4:
						ca2[i] = '4';
						break;
					case 5:
						ca2[i] = '5';
						break;
					case 6:
						ca2[i] = '6';
						break;
					case 7:
						ca2[i] = '7';
						break;
					case 8:
						ca2[i] = '8';
						break;
					case 9:
						ca2[i] = '9';
						break;
				}

				var nZeros = 0;
				if ( !leftJustify && leadingZeros )
				{
					var xThousands = 0;
					if ( thousands )
					{
						var xlead = 0;
						if ( ca2[0] == '+' || ca2[0] == '-' || ca2[0] == ' ' )
							xlead = 1;
						var xdp = xlead;
						for ( ; xdp < ca2.Length; xdp++ )
							if ( ca2[xdp] == '.' )
								break;
						xThousands = ( xdp - xlead ) / 3;
					}

					if ( fieldWidthSet )
						nZeros = fieldWidth - ca2.Length;
					if ( ( !minusSign && ( leadingSign || leadingSpace ) ) || minusSign )
						nZeros--;
					nZeros -= xThousands;
					if ( nZeros < 0 )
						nZeros = 0;
				}

				j = 0;
				if ( ( !minusSign && ( leadingSign || leadingSpace ) ) || minusSign )
				{
					ca3 = new Char[ca2.Length + nZeros + 1];
					j++;
				}
				else
					ca3 = new Char[ca2.Length + nZeros];
				if ( !minusSign )
				{
					if ( leadingSign )
						ca3[0] = '+';
					if ( leadingSpace )
						ca3[0] = ' ';
				}
				else
					ca3[0] = '-';
				for ( k = 0; k < nZeros; j++, k++ )
					ca3[j] = '0';
				for ( i = 0; i < ca2.Length && j < ca3.Length; i++, j++ )
					ca3[j] = ca2[i];
				var lead = 0;
				if ( ca3[0] == '+' || ca3[0] == '-' || ca3[0] == ' ' )
					lead = 1;
				var dp = lead;
				for ( ; dp < ca3.Length; dp++ )
					if ( ca3[dp] == '.' )
						break;
				var nThousands = dp / 3;
				if ( dp < ca3.Length )
					ca3[dp] = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0];
				Char[] ca4 = ca3;
				if ( thousands && nThousands > 0 )
				{
					ca4 = new Char[ca3.Length + nThousands + lead];
					ca4[0] = ca3[0];
					for ( i = lead, k = lead; i < dp; i++ )
					{
						if ( i > 0 && ( dp - i ) % 3 == 0 )
						{
							ca4[k] = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0];
							ca4[k + 1] = ca3[i];
							k += 2;
						}
						else
						{
							ca4[k] = ca3[i];
							k++;
						}
					}

					for ( ; i < ca3.Length; i++, k++ )
						ca4[k] = ca3[i];
				}

				return ca4;
			}

			private Boolean CheckForCarry( Char[] ca1, Int32 icarry )
			{
				var carry = false;
				if ( icarry < ca1.Length )
				{
					if ( ca1[icarry] == '6' || ca1[icarry] == '7' || ca1[icarry] == '8' || ca1[icarry] == '9' )
						carry = true;
					else if ( ca1[icarry] == '5' )
					{
						var ii = icarry + 1;
						for ( ; ii < ca1.Length; ii++ )
							if ( ca1[ii] != '0' )
								break;
						carry = ii < ca1.Length;
						if ( !carry && icarry > 0 )
						{
							carry = ( ca1[icarry - 1] == '1' || ca1[icarry - 1] == '3' || ca1[icarry - 1] == '5' || ca1[icarry - 1] == '7' || ca1[icarry - 1] == '9' );
						}
					}
				}

				return carry;
			}

			private Boolean StartSymbolicCarry( Char[] ca, Int32 cLast, Int32 cFirst )
			{
				var carry = true;
				for ( var i = cLast; carry && i >= cFirst; i-- )
				{
					carry = false;
					switch ( ca[i] )
					{
						case '0':
							ca[i] = '1';
							break;
						case '1':
							ca[i] = '2';
							break;
						case '2':
							ca[i] = '3';
							break;
						case '3':
							ca[i] = '4';
							break;
						case '4':
							ca[i] = '5';
							break;
						case '5':
							ca[i] = '6';
							break;
						case '6':
							ca[i] = '7';
							break;
						case '7':
							ca[i] = '8';
							break;
						case '8':
							ca[i] = '9';
							break;
						case '9':
							ca[i] = '0';
							carry = true;
							break;
					}
				}

				return carry;
			}

			private String EFormatString( Double x, Char eChar )
			{
				var noDigits = false;
				Char[] ca4, ca5;
				if ( Double.IsInfinity( x ) )
				{
					if ( x == Double.PositiveInfinity )
					{
						if ( leadingSign )
							ca4 = "+Inf".ToCharArray();
						else if ( leadingSpace )
							ca4 = " Inf".ToCharArray();
						else
							ca4 = "Inf".ToCharArray();
					}
					else
						ca4 = "-Inf".ToCharArray();
					noDigits = true;
				}
				else if ( Double.IsNaN( x ) )
				{
					if ( leadingSign )
						ca4 = "+NaN".ToCharArray();
					else if ( leadingSpace )
						ca4 = " NaN".ToCharArray();
					else
						ca4 = "NaN".ToCharArray();
					noDigits = true;
				}
				else
					ca4 = EFormatDigits( x, eChar );
				ca5 = ApplyFloatPadding( ca4, false );
				return new String( ca5 );
			}

			private Char[] ApplyFloatPadding( Char[] ca4, Boolean noDigits )
			{
				Char[] ca5 = ca4;
				if ( fieldWidthSet )
				{
					Int32 i, j, nBlanks;
					if ( leftJustify )
					{
						nBlanks = fieldWidth - ca4.Length;
						if ( nBlanks > 0 )
						{
							ca5 = new Char[ca4.Length + nBlanks];
							for ( i = 0; i < ca4.Length; i++ )
								ca5[i] = ca4[i];
							for ( j = 0; j < nBlanks; j++, i++ )
								ca5[i] = ' ';
						}
					}
					else if ( !leadingZeros || noDigits )
					{
						nBlanks = fieldWidth - ca4.Length;
						if ( nBlanks > 0 )
						{
							ca5 = new Char[ca4.Length + nBlanks];
							for ( i = 0; i < nBlanks; i++ )
								ca5[i] = ' ';
							for ( j = 0; j < ca4.Length; i++, j++ )
								ca5[i] = ca4[j];
						}
					}
					else if ( leadingZeros )
					{
						nBlanks = fieldWidth - ca4.Length;
						if ( nBlanks > 0 )
						{
							ca5 = new Char[ca4.Length + nBlanks];
							i = 0;
							j = 0;
							if ( ca4[0] == '-' )
							{
								ca5[0] = '-';
								i++;
								j++;
							}

							for ( var k = 0; k < nBlanks; i++, k++ )
								ca5[i] = '0';
							for ( ; j < ca4.Length; i++, j++ )
								ca5[i] = ca4[j];
						}
					}
				}

				return ca5;
			}

			private String PrintFFormat( Double x )
			{
				return FFormatString( x );
			}

			private String PrintEFormat( Double x )
			{
				if ( conversionCharacter == 'e' )
					return EFormatString( x, 'e' );
				else
					return EFormatString( x, 'E' );
			}

			private String PrintGFormat( Double x )
			{
				String sx, sy, sz, ret;
				var savePrecision = precision;
				Int32 i;
				Char[] ca4, ca5;
				var noDigits = false;
				if ( Double.IsInfinity( x ) )
				{
					if ( x == Double.PositiveInfinity )
					{
						if ( leadingSign )
							ca4 = "+Inf".ToCharArray();
						else if ( leadingSpace )
							ca4 = " Inf".ToCharArray();
						else
							ca4 = "Inf".ToCharArray();
					}
					else
						ca4 = "-Inf".ToCharArray();
					noDigits = true;
				}
				else if ( Double.IsNaN( x ) )
				{
					if ( leadingSign )
						ca4 = "+NaN".ToCharArray();
					else if ( leadingSpace )
						ca4 = " NaN".ToCharArray();
					else
						ca4 = "NaN".ToCharArray();
					noDigits = true;
				}
				else
				{
					if ( !precisionSet )
						precision = defaultDigits;
					if ( precision == 0 )
						precision = 1;
					var ePos = -1;
					if ( conversionCharacter == 'g' )
					{
						sx = EFormatString( x, 'e' ).Trim();
						ePos = sx.IndexOf( 'e' );
					}
					else
					{
						sx = EFormatString( x, 'E' ).Trim();
						ePos = sx.IndexOf( 'E' );
					}

					i = ePos + 1;
					var expon = 0;
					if ( sx[i] == '-' )
					{
						for ( ++i; i < sx.Length; i++ )
							if ( sx[i] != '0' )
								break;
						if ( i < sx.Length )
							expon = -Int32.Parse( sx.Substring( i ) );
					}
					else
					{
						if ( sx[i] == '+' )
							++i;
						for ( ; i < sx.Length; i++ )
							if ( sx[i] != '0' )
								break;
						if ( i < sx.Length )
							expon = Int32.Parse( sx.Substring( i ) );
					}

					if ( !alternateForm )
					{
						if ( expon >= -4 && expon < precision )
							sy = FFormatString( x ).Trim();
						else
							sy = sx.Substring( 0, ePos );
						i = sy.Length - 1;
						for ( ; i >= 0; i-- )
							if ( sy[i] != '0' )
								break;
						if ( i >= 0 && sy[i] == '.' )
							i--;
						if ( i == -1 )
							sz = "0";
						else if ( !Char.IsDigit( sy[i] ) )
							sz = sy.Substring( 0, i + 1 ) + "0";
						else
							sz = sy.Substring( 0, i + 1 );
						if ( expon >= -4 && expon < precision )
							ret = sz;
						else
							ret = sz + sx.Substring( ePos );
					}
					else
					{
						if ( expon >= -4 && expon < precision )
							ret = FFormatString( x ).Trim();
						else
							ret = sx;
					}

					if ( leadingSpace )
						if ( x >= 0 )
							ret = " " + ret;
					ca4 = ret.ToCharArray();
				}

				ca5 = ApplyFloatPadding( ca4, false );
				precision = savePrecision;
				return new String( ca5 );
			}

			private String PrintDFormat( Int16 x )
			{
				return PrintDFormat( x.ToString() );
			}

			private String PrintDFormat( Int64 x )
			{
				return PrintDFormat( x.ToString() );
			}

			private String PrintDFormat( Int32 x )
			{
				return PrintDFormat( x.ToString() );
			}

			private String PrintDFormat( String sx )
			{
				var nLeadingZeros = 0;
				Int32 nBlanks = 0, n = 0;
				Int32 i = 0, jFirst = 0;
				var neg = sx[0] == '-';
				if ( sx.Equals( "0" ) && precisionSet && precision == 0 )
					sx = "";
				if ( !neg )
				{
					if ( precisionSet && sx.Length < precision )
						nLeadingZeros = precision - sx.Length;
				}
				else
				{
					if ( precisionSet && ( sx.Length - 1 ) < precision )
						nLeadingZeros = precision - sx.Length + 1;
				}

				if ( nLeadingZeros < 0 )
					nLeadingZeros = 0;
				if ( fieldWidthSet )
				{
					nBlanks = fieldWidth - nLeadingZeros - sx.Length;
					if ( !neg && ( leadingSign || leadingSpace ) )
						nBlanks--;
				}

				if ( nBlanks < 0 )
					nBlanks = 0;
				if ( leadingSign )
					n++;
				else if ( leadingSpace )
					n++;
				n += nBlanks;
				n += nLeadingZeros;
				n += sx.Length;
				Char[] ca = new Char[n];
				if ( leftJustify )
				{
					if ( neg )
						ca[i++] = '-';
					else if ( leadingSign )
						ca[i++] = '+';
					else if ( leadingSpace )
						ca[i++] = ' ';
					Char[] csx = sx.ToCharArray();
					jFirst = neg ? 1 : 0;
					for ( var j = 0; j < nLeadingZeros; i++, j++ )
						ca[i] = '0';
					for ( var j = jFirst; j < csx.Length; j++, i++ )
						ca[i] = csx[j];
					for ( var j = 0; j < nBlanks; i++, j++ )
						ca[i] = ' ';
				}
				else
				{
					if ( !leadingZeros )
					{
						for ( i = 0; i < nBlanks; i++ )
							ca[i] = ' ';
						if ( neg )
							ca[i++] = '-';
						else if ( leadingSign )
							ca[i++] = '+';
						else if ( leadingSpace )
							ca[i++] = ' ';
					}
					else
					{
						if ( neg )
							ca[i++] = '-';
						else if ( leadingSign )
							ca[i++] = '+';
						else if ( leadingSpace )
							ca[i++] = ' ';
						for ( var j = 0; j < nBlanks; j++, i++ )
							ca[i] = '0';
					}

					for ( var j = 0; j < nLeadingZeros; j++, i++ )
						ca[i] = '0';
					Char[] csx = sx.ToCharArray();
					jFirst = neg ? 1 : 0;
					for ( var j = jFirst; j < csx.Length; j++, i++ )
						ca[i] = csx[j];
				}

				return new String( ca );
			}

			private String PrintXFormat( Int16 x )
			{
				String sx = null;
				if ( x == Int16.MinValue )
					sx = "8000";
				else if ( x < 0 )
				{
					String t;
					if ( x == Int16.MinValue )
						t = "0";
					else
					{
						t = Convert.ToString( ( ~( -x - 1 ) ) ^ Int16.MinValue, 16 );
						if ( t[0] == 'F' || t[0] == 'f' )
							t = t.Substring( 16, 32 );
					}

					switch ( t.Length )
					{
						case 1:
							sx = "800" + t;
							break;
						case 2:
							sx = "80" + t;
							break;
						case 3:
							sx = "8" + t;
							break;
						case 4:
							switch ( t[0] )
							{
								case '1':
									sx = "9" + t.Substring( 1, 4 );
									break;
								case '2':
									sx = "a" + t.Substring( 1, 4 );
									break;
								case '3':
									sx = "b" + t.Substring( 1, 4 );
									break;
								case '4':
									sx = "c" + t.Substring( 1, 4 );
									break;
								case '5':
									sx = "d" + t.Substring( 1, 4 );
									break;
								case '6':
									sx = "e" + t.Substring( 1, 4 );
									break;
								case '7':
									sx = "f" + t.Substring( 1, 4 );
									break;
							}

							break;
					}
				}
				else
					sx = Convert.ToString( ( Int32 ) x, 16 );
				return PrintXFormat( sx );
			}

			private String PrintXFormat( Int64 x )
			{
				String sx = null;
				if ( x == Int64.MinValue )
					sx = "8000000000000000";
				else if ( x < 0 )
				{
					var t = Convert.ToString( ( ~( -x - 1 ) ) ^ Int64.MinValue, 16 );
					switch ( t.Length )
					{
						case 1:
							sx = "800000000000000" + t;
							break;
						case 2:
							sx = "80000000000000" + t;
							break;
						case 3:
							sx = "8000000000000" + t;
							break;
						case 4:
							sx = "800000000000" + t;
							break;
						case 5:
							sx = "80000000000" + t;
							break;
						case 6:
							sx = "8000000000" + t;
							break;
						case 7:
							sx = "800000000" + t;
							break;
						case 8:
							sx = "80000000" + t;
							break;
						case 9:
							sx = "8000000" + t;
							break;
						case 10:
							sx = "800000" + t;
							break;
						case 11:
							sx = "80000" + t;
							break;
						case 12:
							sx = "8000" + t;
							break;
						case 13:
							sx = "800" + t;
							break;
						case 14:
							sx = "80" + t;
							break;
						case 15:
							sx = "8" + t;
							break;
						case 16:
							switch ( t[0] )
							{
								case '1':
									sx = "9" + t.Substring( 1, 16 );
									break;
								case '2':
									sx = "a" + t.Substring( 1, 16 );
									break;
								case '3':
									sx = "b" + t.Substring( 1, 16 );
									break;
								case '4':
									sx = "c" + t.Substring( 1, 16 );
									break;
								case '5':
									sx = "d" + t.Substring( 1, 16 );
									break;
								case '6':
									sx = "e" + t.Substring( 1, 16 );
									break;
								case '7':
									sx = "f" + t.Substring( 1, 16 );
									break;
							}

							break;
					}
				}
				else
					sx = Convert.ToString( x, 16 );
				return PrintXFormat( sx );
			}

			private String PrintXFormat( Int32 x )
			{
				String sx = null;
				if ( x == Int32.MinValue )
					sx = "80000000";
				else if ( x < 0 )
				{
					var t = Convert.ToString( ( ~( -x - 1 ) ) ^ Int32.MinValue, 16 );
					switch ( t.Length )
					{
						case 1:
							sx = "8000000" + t;
							break;
						case 2:
							sx = "800000" + t;
							break;
						case 3:
							sx = "80000" + t;
							break;
						case 4:
							sx = "8000" + t;
							break;
						case 5:
							sx = "800" + t;
							break;
						case 6:
							sx = "80" + t;
							break;
						case 7:
							sx = "8" + t;
							break;
						case 8:
							switch ( t[0] )
							{
								case '1':
									sx = "9" + t.Substring( 1, 8 );
									break;
								case '2':
									sx = "a" + t.Substring( 1, 8 );
									break;
								case '3':
									sx = "b" + t.Substring( 1, 8 );
									break;
								case '4':
									sx = "c" + t.Substring( 1, 8 );
									break;
								case '5':
									sx = "d" + t.Substring( 1, 8 );
									break;
								case '6':
									sx = "e" + t.Substring( 1, 8 );
									break;
								case '7':
									sx = "f" + t.Substring( 1, 8 );
									break;
							}

							break;
					}
				}
				else
					sx = Convert.ToString( x, 16 );
				return PrintXFormat( sx );
			}

			private String PrintXFormat( String sx )
			{
				var nLeadingZeros = 0;
				var nBlanks = 0;
				if ( sx.Equals( "0" ) && precisionSet && precision == 0 )
					sx = "";
				if ( precisionSet )
					nLeadingZeros = precision - sx.Length;
				if ( nLeadingZeros < 0 )
					nLeadingZeros = 0;
				if ( fieldWidthSet )
				{
					nBlanks = fieldWidth - nLeadingZeros - sx.Length;
					if ( alternateForm )
						nBlanks = nBlanks - 2;
				}

				if ( nBlanks < 0 )
					nBlanks = 0;
				var n = 0;
				if ( alternateForm )
					n += 2;
				n += nLeadingZeros;
				n += sx.Length;
				n += nBlanks;
				Char[] ca = new Char[n];
				var i = 0;
				if ( leftJustify )
				{
					if ( alternateForm )
					{
						ca[i++] = '0';
						ca[i++] = 'x';
					}

					for ( var j = 0; j < nLeadingZeros; j++, i++ )
						ca[i] = '0';
					Char[] csx = sx.ToCharArray();
					for ( var j = 0; j < csx.Length; j++, i++ )
						ca[i] = csx[j];
					for ( var j = 0; j < nBlanks; j++, i++ )
						ca[i] = ' ';
				}
				else
				{
					if ( !leadingZeros )
						for ( var j = 0; j < nBlanks; j++, i++ )
							ca[i] = ' ';
					if ( alternateForm )
					{
						ca[i++] = '0';
						ca[i++] = 'x';
					}

					if ( leadingZeros )
						for ( var j = 0; j < nBlanks; j++, i++ )
							ca[i] = '0';
					for ( var j = 0; j < nLeadingZeros; j++, i++ )
						ca[i] = '0';
					Char[] csx = sx.ToCharArray();
					for ( var j = 0; j < csx.Length; j++, i++ )
						ca[i] = csx[j];
				}

				var caReturn = new String( ca );
				if ( conversionCharacter == 'X' )
					caReturn = caReturn.ToUpper();
				return caReturn;
			}

			private String PrintOFormat( Int16 x )
			{
				String sx = null;
				if ( x == Int16.MinValue )
					sx = "100000";
				else if ( x < 0 )
				{
					var t = Convert.ToString( ( ~( -x - 1 ) ) ^ Int16.MinValue, 8 );
					switch ( t.Length )
					{
						case 1:
							sx = "10000" + t;
							break;
						case 2:
							sx = "1000" + t;
							break;
						case 3:
							sx = "100" + t;
							break;
						case 4:
							sx = "10" + t;
							break;
						case 5:
							sx = "1" + t;
							break;
					}
				}
				else
					sx = Convert.ToString( ( Int32 ) x, 8 );
				return PrintOFormat( sx );
			}

			private String PrintOFormat( Int64 x )
			{
				String sx = null;
				if ( x == Int64.MinValue )
					sx = "1000000000000000000000";
				else if ( x < 0 )
				{
					var t = Convert.ToString( ( ~( -x - 1 ) ) ^ Int64.MinValue, 8 );
					switch ( t.Length )
					{
						case 1:
							sx = "100000000000000000000" + t;
							break;
						case 2:
							sx = "10000000000000000000" + t;
							break;
						case 3:
							sx = "1000000000000000000" + t;
							break;
						case 4:
							sx = "100000000000000000" + t;
							break;
						case 5:
							sx = "10000000000000000" + t;
							break;
						case 6:
							sx = "1000000000000000" + t;
							break;
						case 7:
							sx = "100000000000000" + t;
							break;
						case 8:
							sx = "10000000000000" + t;
							break;
						case 9:
							sx = "1000000000000" + t;
							break;
						case 10:
							sx = "100000000000" + t;
							break;
						case 11:
							sx = "10000000000" + t;
							break;
						case 12:
							sx = "1000000000" + t;
							break;
						case 13:
							sx = "100000000" + t;
							break;
						case 14:
							sx = "10000000" + t;
							break;
						case 15:
							sx = "1000000" + t;
							break;
						case 16:
							sx = "100000" + t;
							break;
						case 17:
							sx = "10000" + t;
							break;
						case 18:
							sx = "1000" + t;
							break;
						case 19:
							sx = "100" + t;
							break;
						case 20:
							sx = "10" + t;
							break;
						case 21:
							sx = "1" + t;
							break;
					}
				}
				else
					sx = Convert.ToString( x, 8 );
				return PrintOFormat( sx );
			}

			private String PrintOFormat( Int32 x )
			{
				String sx = null;
				if ( x == Int32.MinValue )
					sx = "20000000000";
				else if ( x < 0 )
				{
					var t = Convert.ToString( ( ~( -x - 1 ) ) ^ Int32.MinValue, 8 );
					switch ( t.Length )
					{
						case 1:
							sx = "2000000000" + t;
							break;
						case 2:
							sx = "200000000" + t;
							break;
						case 3:
							sx = "20000000" + t;
							break;
						case 4:
							sx = "2000000" + t;
							break;
						case 5:
							sx = "200000" + t;
							break;
						case 6:
							sx = "20000" + t;
							break;
						case 7:
							sx = "2000" + t;
							break;
						case 8:
							sx = "200" + t;
							break;
						case 9:
							sx = "20" + t;
							break;
						case 10:
							sx = "2" + t;
							break;
						case 11:
							sx = "3" + t.Substring( 1 );
							break;
					}
				}
				else
					sx = Convert.ToString( x, 8 );
				return PrintOFormat( sx );
			}

			private String PrintOFormat( String sx )
			{
				var nLeadingZeros = 0;
				var nBlanks = 0;
				if ( sx.Equals( "0" ) && precisionSet && precision == 0 )
					sx = "";
				if ( precisionSet )
					nLeadingZeros = precision - sx.Length;
				if ( alternateForm )
					nLeadingZeros++;
				if ( nLeadingZeros < 0 )
					nLeadingZeros = 0;
				if ( fieldWidthSet )
					nBlanks = fieldWidth - nLeadingZeros - sx.Length;
				if ( nBlanks < 0 )
					nBlanks = 0;
				var n = nLeadingZeros + sx.Length + nBlanks;
				Char[] ca = new Char[n];
				Int32 i;
				if ( leftJustify )
				{
					for ( i = 0; i < nLeadingZeros; i++ )
						ca[i] = '0';
					Char[] csx = sx.ToCharArray();
					for ( var j = 0; j < csx.Length; j++, i++ )
						ca[i] = csx[j];
					for ( var j = 0; j < nBlanks; j++, i++ )
						ca[i] = ' ';
				}
				else
				{
					if ( leadingZeros )
						for ( i = 0; i < nBlanks; i++ )
							ca[i] = '0';
					else
						for ( i = 0; i < nBlanks; i++ )
							ca[i] = ' ';
					for ( var j = 0; j < nLeadingZeros; j++, i++ )
						ca[i] = '0';
					Char[] csx = sx.ToCharArray();
					for ( var j = 0; j < csx.Length; j++, i++ )
						ca[i] = csx[j];
				}

				return new String( ca );
			}

			private String PrintCFormat( Char x )
			{
				var nPrint = 1;
				var width = fieldWidth;
				if ( !fieldWidthSet )
					width = nPrint;
				Char[] ca = new Char[width];
				var i = 0;
				if ( leftJustify )
				{
					ca[0] = x;
					for ( i = 1; i <= width - nPrint; i++ )
						ca[i] = ' ';
				}
				else
				{
					for ( i = 0; i < width - nPrint; i++ )
						ca[i] = ' ';
					ca[i] = x;
				}

				return new String( ca );
			}

			private String PrintSFormat( String x )
			{
				var nPrint = x.Length;
				var width = fieldWidth;
				if ( precisionSet && nPrint > precision )
					nPrint = precision;
				if ( !fieldWidthSet )
					width = nPrint;
				var n = 0;
				if ( width > nPrint )
					n += width - nPrint;
				if ( nPrint >= x.Length )
					n += x.Length;
				else
					n += nPrint;
				Char[] ca = new Char[n];
				var i = 0;
				if ( leftJustify )
				{
					if ( nPrint >= x.Length )
					{
						Char[] csx = x.ToCharArray();
						for ( i = 0; i < x.Length; i++ )
							ca[i] = csx[i];
					}
					else
					{
						Char[] csx = x.Substring( 0, nPrint ).ToCharArray();
						for ( i = 0; i < nPrint; i++ )
							ca[i] = csx[i];
					}

					for ( var j = 0; j < width - nPrint; j++, i++ )
						ca[i] = ' ';
				}
				else
				{
					for ( i = 0; i < width - nPrint; i++ )
						ca[i] = ' ';
					if ( nPrint >= x.Length )
					{
						Char[] csx = x.ToCharArray();
						for ( var j = 0; j < x.Length; i++, j++ )
							ca[i] = csx[j];
					}
					else
					{
						Char[] csx = x.Substring( 0, nPrint ).ToCharArray();
						for ( var j = 0; j < nPrint; i++, j++ )
							ca[i] = csx[j];
					}
				}

				return new String( ca );
			}

			private Boolean SetConversionCharacter( )
			{
				var ret = false;
				conversionCharacter = '\\';
				if ( pos < fmt.Length )
				{
					var c = fmt[pos];
					if ( c == 'i' || c == 'd' || c == 'f' || c == 'g' || c == 'G' || c == 'o' || c == 'x' || c == 'X' || c == 'e' || c == 'E' || c == 'c' || c == 's' || c == '%' )
					{
						conversionCharacter = c;
						pos++;
						ret = true;
					}
				}

				return ret;
			}

			private void SetOptionalHL( )
			{
				optionalh = false;
				optionall = false;
				optionalL = false;
				if ( pos < fmt.Length )
				{
					var c = fmt[pos];
					if ( c == 'h' )
					{
						optionalh = true;
						pos++;
					}
					else if ( c == 'l' )
					{
						optionall = true;
						pos++;
					}
					else if ( c == 'L' )
					{
						optionalL = true;
						pos++;
					}
				}
			}

			private void SetPrecision( )
			{
				var firstPos = pos;
				precisionSet = false;
				if ( pos < fmt.Length && fmt[pos] == '.' )
				{
					pos++;
					if ( ( pos < fmt.Length ) && ( fmt[pos] == '*' ) )
					{
						pos++;
						if ( !SetPrecisionArgPosition() )
						{
							variablePrecision = true;
							precisionSet = true;
						}

						return;
					}
					else
					{
						while ( pos < fmt.Length )
						{
							var c = fmt[pos];
							if ( Char.IsDigit( c ) )
								pos++;
							else
								break;
						}

						if ( pos > firstPos + 1 )
						{
							var sz = fmt.Substring( firstPos + 1, pos );
							precision = Int32.Parse( sz );
							precisionSet = true;
						}
					}
				}
			}

			private void SetFieldWidth( )
			{
				var firstPos = pos;
				fieldWidth = 0;
				fieldWidthSet = false;
				if ( ( pos < fmt.Length ) && ( fmt[pos] == '*' ) )
				{
					pos++;
					if ( !SetFieldWidthArgPosition() )
					{
						variableFieldWidth = true;
						fieldWidthSet = true;
					}
				}
				else
				{
					while ( pos < fmt.Length )
					{
						var c = fmt[pos];
						if ( Char.IsDigit( c ) )
							pos++;
						else
							break;
					}

					if ( firstPos < pos && firstPos < fmt.Length )
					{
						var sz = fmt.Substring( firstPos, pos );
						fieldWidth = Int32.Parse( sz );
						fieldWidthSet = true;
					}
				}
			}

			private void SetArgPosition( )
			{
				Int32 xPos;
				for ( xPos = pos; xPos < fmt.Length; xPos++ )
				{
					if ( !Char.IsDigit( fmt[xPos] ) )
						break;
				}

				if ( xPos > pos && xPos < fmt.Length )
				{
					if ( fmt[xPos] == '$' )
					{
						positionalSpecification = true;
						argumentPosition = Int32.Parse( fmt.Substring( pos, xPos ) );
						pos = xPos + 1;
					}
				}
			}

			private Boolean SetFieldWidthArgPosition( )
			{
				var ret = false;
				Int32 xPos;
				for ( xPos = pos; xPos < fmt.Length; xPos++ )
				{
					if ( !Char.IsDigit( fmt[xPos] ) )
						break;
				}

				if ( xPos > pos && xPos < fmt.Length )
				{
					if ( fmt[xPos] == '$' )
					{
						positionalFieldWidth = true;
						argumentPositionForFieldWidth = Int32.Parse( fmt.Substring( pos, xPos ) );
						pos = xPos + 1;
						ret = true;
					}
				}

				return ret;
			}

			private Boolean SetPrecisionArgPosition( )
			{
				var ret = false;
				Int32 xPos;
				for ( xPos = pos; xPos < fmt.Length; xPos++ )
				{
					if ( !Char.IsDigit( fmt[xPos] ) )
						break;
				}

				if ( xPos > pos && xPos < fmt.Length )
				{
					if ( fmt[xPos] == '$' )
					{
						positionalPrecision = true;
						argumentPositionForPrecision = Int32.Parse( fmt.Substring( pos, xPos ) );
						pos = xPos + 1;
						ret = true;
					}
				}

				return ret;
			}

			public virtual Boolean IsPositionalSpecification( )
			{
				return positionalSpecification;
			}

			public virtual Int32 GetArgumentPosition( )
			{
				return argumentPosition;
			}

			public virtual Boolean IsPositionalFieldWidth( )
			{
				return positionalFieldWidth;
			}

			public virtual Int32 GetArgumentPositionForFieldWidth( )
			{
				return argumentPositionForFieldWidth;
			}

			public virtual Boolean IsPositionalPrecision( )
			{
				return positionalPrecision;
			}

			public virtual Int32 GetArgumentPositionForPrecision( )
			{
				return argumentPositionForPrecision;
			}

			private void SetFlagCharacters( )
			{
				thousands = false;
				leftJustify = false;
				leadingSign = false;
				leadingSpace = false;
				alternateForm = false;
				leadingZeros = false;
				for ( ; pos < fmt.Length; pos++ )
				{
					var c = fmt[pos];
					if ( c == '\\' )
						thousands = true;
					else if ( c == '-' )
					{
						leftJustify = true;
						leadingZeros = false;
					}
					else if ( c == '+' )
					{
						leadingSign = true;
						leadingSpace = false;
					}
					else if ( c == ' ' )
					{
						if ( !leadingSign )
							leadingSpace = true;
					}
					else if ( c == '#' )
						alternateForm = true;
					else if ( c == '0' )
					{
						if ( !leftJustify )
							leadingZeros = true;
					}
					else
						break;
				}
			}

			private Boolean thousands = false;
			private Boolean leftJustify = false;
			private Boolean leadingSign = false;
			private Boolean leadingSpace = false;
			private Boolean alternateForm = false;
			private Boolean leadingZeros = false;
			private Boolean variableFieldWidth = false;
			private Int32 fieldWidth = 0;
			private Boolean fieldWidthSet = false;
			private Int32 precision = 0;
			private static readonly Int32 defaultDigits = 6;
			private Boolean variablePrecision = false;
			private Boolean precisionSet = false;
			private Boolean positionalSpecification = false;
			private Int32 argumentPosition = 0;
			private Boolean positionalFieldWidth = false;
			private Int32 argumentPositionForFieldWidth = 0;
			private Boolean positionalPrecision = false;
			private Int32 argumentPositionForPrecision = 0;
			private Boolean optionalh = false;
			private Boolean optionall = false;
			private Boolean optionalL = false;
			private Char conversionCharacter = '\\';
			private Int32 pos = 0;
			private String fmt;
		}

		private ArrayList vFmt = new ArrayList();
		private Int32 cPos = 0;
	}
}