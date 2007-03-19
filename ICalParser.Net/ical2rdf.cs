using System;
using System.Text;
using System.Collections;
using System.IO;
using System.Net;
using System.Web;

/***
 * <copyright>
 *   ICalParser is a general purpose .Net parser for iCalendar format files (RFC 2445)
 * 
 *   ical2rdf.exe is a command line executable used for converting iCalendar (RFC 2445) files
 *     to iCalendar/RDF format (see http://www.w3.org/2002/12/cal/ for more info on the W3C
 *     working group responsible for iCalendar/RDF)
 * 
 *   Copyright (C) 2004  J. Tim Spurway
 *
 *   This program is free software; you can redistribute it and/or modify
 *   it under the terms of the GNU General Public License as published by
 *   the Free Software Foundation; either version 2 of the License, or
 *   (at your option) any later version.
 *
 *   This program is distributed in the hope that it will be useful,
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *   GNU General Public License for more details.
 *
 *   You should have received a copy of the GNU General Public License
 *   along with this program; if not, write to the Free Software
 *   Foundation, Inc., 675 Mass Ave, Cambridge, MA 02139, USA.
 * </copyright>
 */

namespace Semaview.Shared.ICalParser
{
    /// <summary>
    /// Converts iCalendar files (RFC 2445) to iCalendar RDF.
    /// </summary>
    /// <remarks>
    /// usage:
    ///	    ical2rdf [-chp][-x | -f rdffile] icalfile1 [icalfile2 ... icalfileN]
    ///	    
    ///	    options:
    ///		c - print copyright information
    ///		h - print help/usage information
    ///		p - supress output of error messages - will force a file to be written out
    ///		    regardless of error conditions (should be used for debugging only)
    ///		x - name all rdf output files with the same name as their corresponding
    ///		    iCalendar files, except with an '.rdf' file suffix.  For URLs filenames 
    ///		    like 'calendar0.rdf' 'calendar1.rdf' etc. will be chosen.
    ///		f rdffile - generate the RDF file with the given name.  If multiple iCalendar
    ///		    files are specified, this option is ignored
    ///		icalfileN - the iCalendar (RFC2445) file(s) that are to be parsed.  These must
    ///		    be the last parameters in the command.  This program will not expand wildcard
    ///		    characters or do anything else that the shell is supposed to do...
    ///		    
    ///		    NOTE: icalfiles will be assumed to be files on a local filesystem or mounted
    ///		    network drives.  If you prefix the name with 'http:', it will be
    ///		    fetched from the internet URL supplied. No attempt will be made to do any 
    ///		    Authentication for internet fetches.
    ///	    
    ///	generate an RDF iCalendar format file based on iCalendar file input
    ///	    
    ///	example:
    ///	    ical2rdf myCal.ics                  -- generates RDF on stdout
    ///	    ical2rdf myCal.ics yourCal.ics      -- generates RDF on stdout
    ///	    ical2rdf -x myCal.ics               -- generates the file 'myCal.rdf'
    ///	    ical2rdf -f yourCal.xml myCal.ics   -- generates the file 'yourCal.xml'
    /// </remarks>
    class ICal2Rdf
    {
	/// <summary>
	/// The main entry point for the application.
	/// </summary>
	[STAThread]
	static void Main( string[] args )
	{
	    if( args.Length < 1 )
	    {
		Usage();
	    }
	    else
	    {
		ArrayList icalFiles = new ArrayList();
		bool startedFilesPart = false;
		bool supressErrors = false;
		bool outputSameName = false;
		string rdfFilename = null;
		int urlcounter = 0;

		// first process the flags and arguments
		for( int i = 0; i < args.Length; i++ )
		{
		    string arg = args[i];

		    if( arg.StartsWith( "-" ) && !startedFilesPart )
		    {
			if( arg.Length == 2 )
			{
			    switch( arg[1] )
			    {
				case 'c':
				    Copyright();
				    return;
				case 'h':
				    Usage();
				    return;
				case 'p':
				    supressErrors = true;
				    break;
				case 'x':
				    outputSameName = true;
				    break;
				case 'f':
				    if( i+1 >= args.Length )
				    {
					Usage();
					return;
				    }
				    rdfFilename = args[ ++i ];
				    break;
			    }
			}
			else
			{
			    Usage();
			    return;
			}
		    }
		    else
		    {
			startedFilesPart = true;  // ignore any options in the
			icalFiles.Add( arg );
		    }
		}

		// deactivate the -f flag if there are more than one icsfiles - assume they want
		// file output, which will mean the -x flag...
		if( rdfFilename != null && icalFiles.Count > 1 )
		{
		    rdfFilename = null;
		    outputSameName = true;
		}

		// now loop through the ics files and translate them to RDF
		foreach( string file in icalFiles )
		{
		    TextReader reader;
		    bool isURL = false;

		    try
		    {
			if( file.StartsWith( "http:" ))
			{
			    isURL = true;
			    HttpWebRequest req = (HttpWebRequest)WebRequest.Create( file );
			    req.Proxy = WebProxy.GetDefaultProxy();
			    WebResponse resp = req.GetResponse();

			    // TODO: Found a 'bug' or 'feature' (depending on perspective), of the StreamReader.Peek()
			    // method.  It does not work on streams that are socket (network) based.  It will 
			    // report EOF at the end of packet, instead of end of file.  So - let's read in the
			    // entire stream into memory, and rework the Scanner class so that it doesn't use
			    // Peek().  
			    StreamReader tempReader = new StreamReader( resp.GetResponseStream( ));
			    reader = new StringReader( tempReader.ReadToEnd( ));
			}
			else
			{
			    reader = new StreamReader( file );
			}
		    }
		    catch( Exception e )
		    {
			Console.Error.WriteLine( "Error encountered processing file: " + file + "\n   message: " + e.Message );
			Usage();
			return;
		    }

		    RDFEmitter emitter = new RDFEmitter( );
		    Parser parser = new Parser( reader, emitter );
		    parser.Parse( );

		    // check for errors
		    if( !supressErrors && parser.HasErrors )
		    {
			foreach( ParserError error in parser.Errors )
			{
			    Console.Error.WriteLine( "Error on line: " + error.Linenumber + ".  " + error.Message );
			}
		    }
		    else
		    {
			// output it!
			TextWriter writer;

			if( rdfFilename != null )
			{
			    writer = new StreamWriter( rdfFilename );
			}
			else if( outputSameName )
			{
			    string outfile, infile;
			    if( isURL )
			    {
				outfile = "calendar" + urlcounter + ".rdf";
				urlcounter++;
			    }
			    else
			    {
				int fslashindex = file.LastIndexOf( '/' );
				int bslashindex = file.LastIndexOf( '\\' );
				int slashindex = Math.Max( fslashindex, bslashindex );

				if( slashindex >= 0 )
				{
				    infile = file.Substring( slashindex+1 );
				}
				else
				{
				    infile = file;
				}
			    
				int dotindex = infile.LastIndexOf( '.' );
				if( dotindex == -1 )
				{
				    outfile = infile + ".rdf";
				}
				else
				{
				    outfile = infile.Substring( 0, dotindex) + ".rdf";
				}
			    }
			    writer = new StreamWriter( outfile );
			}
			else
			{
			    writer = Console.Out;
			}

			writer.Write( emitter.Rdf );
		    }
		}
	    }
	}

	private static void Usage()
	{
	    Console.Error.WriteLine(
@"usage:

    ical2rdf [-chp][-x | -f rdffile] icalfile1 [icalfile2 ... icalfileN]

	options:
	    c - print copyright information
	    h - print help/usage information
	    p - supress output of error messages - will force a file to be 
		written out regardless of error conditions (should be used 
		for debugging only)
	    x - name all rdf output files with the same name as their 
		corresponding iCalendar files, except with an '.rdf' suffix.
                For URLs filenames like 'calendar0.rdf' 'calendar1.rdf' 
                etc. will be chosen.
	    f rdffile - generate the RDF file with the given name.  If 
		multiple iCalendar files are specified this option is ignored
	    icalfileN - the iCalendar (RFC2445) file(s) that are to be parsed.
		These must be the last parameters in the command.  This 
		program will not expand wildcard characters

		NOTE: icalfiles will be assumed to be files on a local 
		filesystem or mounted network drives.  If you prefix the 
		name with 'http:', it will be fetched from the internet 
		URL supplied. No attempt will be made to do any Authentication 
		for internet fetches.

" );
	    Copyright();
	}

	private static void Copyright()
	{
	    Console.Error.WriteLine(
@"    ICalParser.Net version 1, Copyright (C) 2004 J. Tim Spurway
    ICalParser.Net comes with ABSOLUTELY NO WARRANTY; 
    for details see the file 'COPYING' in this distribution.
    This is free software, and you are welcome to redistribute it
    under certain conditions; 
    see the file 'COPYING' for more info about the GNU Public License." );
	}
    }
}
