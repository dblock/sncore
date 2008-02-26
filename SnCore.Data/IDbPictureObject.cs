using System;
using System.Collections.Generic;
using System.Text;

public interface IDbPictureObject
{
    System.Int32 Id { get; }
    System.Int32 Position { get; set; }
    System.DateTime Created { get; set; }
}
