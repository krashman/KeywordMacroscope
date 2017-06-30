﻿/*

	This file is part of SEOMacroscope.

	Copyright 2017 Jason Holland.

	The GitHub repository may be found at:

		https://github.com/nazuke/SEOMacroscope

	Foobar is free software: you can redistribute it and/or modify
	it under the terms of the GNU General Public License as published by
	the Free Software Foundation, either version 3 of the License, or
	(at your option) any later version.

	Foobar is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
	GNU General Public License for more details.

	You should have received a copy of the GNU General Public License
	along with Foobar.  If not, see <http://www.gnu.org/licenses/>.

*/

using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Runtime;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Text;

namespace KeywordMacroscope
{

  /// <summary>
  /// Description of MacroscopeBase.
  /// </summary>

  public class MacroscopeBase
  {

    /**************************************************************************/

    // Override SuppressDebugMsg to enable/disable debugging output statically.
    public static Boolean SuppressStaticDebugMsg;

    // Set SuppressDebugMsg to enable/disable debugging output in object.
    public Boolean SuppressDebugMsg;

    private string UserAgentString;

    protected static Dictionary<string,string> Memoize = new Dictionary<string,string> ( 1024 );

    protected static Boolean ThrowInsufficientMemoryException;

    /**************************************************************************/

    static MacroscopeBase ()
    {

      SuppressStaticDebugMsg = false;

      ThrowInsufficientMemoryException = false;

    }

    public MacroscopeBase ()
    {
      
      this.SuppressDebugMsg = false;
      
      this.UserAgentString = this._UserAgent();
      
    }

    /**************************************************************************/
    
    private static string GetVersion ()
    {
      string Location = Assembly.GetExecutingAssembly().Location;
      string Version = FileVersionInfo.GetVersionInfo( Location ).ProductVersion;
      return( Version );
    }

    /** HTTP User Agent *******************************************************/
    
    public string UserAgent ()
    {
      return( this.UserAgentString );
    }
    
    /** -------------------------------------------------------------------- **/
        
    private string _UserAgent ()
    {
      #if (DEBUG)
      const string MyUserAgent = "BOT";
      #else
      string Location = Assembly.GetExecutingAssembly().Location;
      string Name = FileVersionInfo.GetVersionInfo( Location ).ProductName;
      string Version = FileVersionInfo.GetVersionInfo( Location ).ProductVersion;
      string MyUserAgent = string.Format( "{0}/{1}", Name, Version );
      #endif
      return( MyUserAgent );
    }

    /** Text Digest ***********************************************************/

    public static string GetStringDigest ( string Text )
    {

      string Digested = null;

      if( MacroscopeBase.Memoize.ContainsKey( Text ) )
      {
        Digested = MacroscopeBase.Memoize[ Text ];
      }
      else
      {

        HashAlgorithm Digest = HashAlgorithm.Create( "MD5" );
        byte [] BytesIn = Encoding.UTF8.GetBytes( Text );
        byte [] Hashed = Digest.ComputeHash( BytesIn );
        StringBuilder Buf = new StringBuilder ();

        for( int i = 0 ; i < Hashed.Length ; i++ )
        {
          Buf.Append( Hashed[ i ].ToString( "X2" ) );
        }

        Digested = Buf.ToString();

        MacroscopeBase.Memoize[ Text ] = Digested;

      }

      return( Digested );
    }

    /** Memory Gate ***********************************************************/

    // https://msdn.microsoft.com/en-us/library/system.runtime.gcsettings.largeobjectheapcompactionmode%28v=vs.110%29.aspx
       
    /** EXAMPLE:

      try
      {
        if( this.MemoryGate( RequiredMegabytes: 32 ) )
        {
          // DO STUFF HERE
        }
      }
      catch( MacroscopeInsufficientMemoryException ex )
      {
        DebugMsg( string.Format( "MacroscopeInsufficientMemoryException: {0}", ex.Message ) );
        GC.Collect();
      }

   **/

    /** -------------------------------------------------------------------- **/

    public static void EnableThrowInsufficientMemoryException ()
    {
      ThrowInsufficientMemoryException = true;
    }

    public static void DisableThrowInsufficientMemoryException ()
    {
      ThrowInsufficientMemoryException = true;
    }

    /** -------------------------------------------------------------------- **/
		/*
    protected Boolean MemoryGate ( int RequiredMegabytes )
    {

      GC.Collect();
              
      if( MacroscopePreferencesManager.GetEnableMemoryGuard() )
      {

        MemoryFailPoint MemGate = null;

        try
        {

          DebugMsg( string.Format( "RequiredMegabytes: {0}", RequiredMegabytes ) );

          MemGate = new MemoryFailPoint ( RequiredMegabytes );

        }
        catch( InsufficientMemoryException ex )
        {

          if( ThrowInsufficientMemoryException )
          {
        
            throw new MacroscopeInsufficientMemoryException (
              message: string.Format( "Insufficient memory available: {0}MB is required", RequiredMegabytes ),
              innerException: ex
            );
          
          }
        
        }
      
        GC.Collect();

        if( MemGate != null )
        {
          return( true );
        }
      
        return( false );
      
      }

      return( true );
      
    }
    */
    /** -------------------------------------------------------------------- **/
    /*
    public static Boolean MemoryGuard ( int RequiredMegabytes )
    {

      GC.Collect();
              
      if( MacroscopePreferencesManager.GetEnableMemoryGuard() )
      {

        MemoryFailPoint MemGate = null;

        try
        {

          DebugMsg( string.Format( "RequiredMegabytes: {0}", RequiredMegabytes ), true );

          MemGate = new MemoryFailPoint ( RequiredMegabytes );

        }
        catch( InsufficientMemoryException ex )
        {

          if( ThrowInsufficientMemoryException )
          {

            throw new MacroscopeInsufficientMemoryException (
              message: string.Format( "Insufficient memory available: {0}MB is required", RequiredMegabytes ),
              innerException: ex
            );
                  
          }
        
        }
      
        GC.Collect();

        if( MemGate != null )
        {
          return( true );
        }
            
        return( false );
      
      }
      
      return( true );

    }
		*/
    /**************************************************************************/
		
    [Conditional( "DEVMODE" )]
    public static void DebugMsg ( string Msg, Boolean Flag )
    {
      if( !SuppressStaticDebugMsg )
      {
        Debug.WriteLine( Msg );
      }
    }

    [Conditional( "DEVMODE" )]
    public void DebugMsg ( string Msg )
    {
      if( !SuppressDebugMsg )
      {
        Debug.WriteLine(
          string.Format(
            "TID:{0} :: {1} :: {2}",
            Thread.CurrentThread.ManagedThreadId,
            this.GetType(),
            Msg
          )
        );
      }
    }
    
    [Conditional( "DEVMODE" )]
    public void DebugMsgForced ( string Msg )
    {
      Debug.WriteLine(
        string.Format(
          "TID:{0} :: {1} :: {2}",
          Thread.CurrentThread.ManagedThreadId,
          this.GetType(),
          Msg
        )
      );
    }

    /**************************************************************************/

  }

}
