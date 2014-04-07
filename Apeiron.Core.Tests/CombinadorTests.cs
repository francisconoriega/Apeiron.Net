using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Apeiron.Core.Tests
{
    [TestClass]
    public class CombinadorTests
    {
        #region data
        string data = @"[
  {
    ""Clave"": ""CC101"",
    ""Calendario"": ""201410"",
    ""Nombre"": ""TALLER DE INTRODUCCION A LA COMPUTACION"",
    ""Creditos"": 3,
    ""Grupos"": [
      {
        ""CentroUniversitario"": ""D"",
        ""Maestro"": ""0700-0855"",
        ""Seccion"": ""D04"",
        ""NRC"": ""06827"",
        ""Cupo"": 20,
        ""CupoDisponible"": 5,
        ""Periodo"": ""04/02/14 - 13/06/14"",
        ""LugaresHoras"": [
          {
            ""HoraInicio"": {
              ""ticks"": 252000000000
            },
            ""HoraFin"": {
              ""ticks"": 321000000000
            },
            ""Dias"": [
              5
            ],
            ""Edificio"": ""DUCT2"",
            ""Aula"": ""LC07""
          }
        ]
      },
      {
        ""CentroUniversitario"": ""D"",
        ""Maestro"": ""1300-1455"",
        ""Seccion"": ""D07"",
        ""NRC"": ""06830"",
        ""Cupo"": 20,
        ""CupoDisponible"": 0,
        ""Periodo"": ""04/02/14 - 13/06/14"",
        ""LugaresHoras"": [
          {
            ""HoraInicio"": {
              ""ticks"": 468000000000
            },
            ""HoraFin"": {
              ""ticks"": 537000000000
            },
            ""Dias"": [
              1
            ],
            ""Edificio"": ""DUCT2"",
            ""Aula"": ""LC06""
          }
        ]
      },
      {
        ""CentroUniversitario"": ""D"",
        ""Maestro"": ""0800-0955"",
        ""Seccion"": ""D10"",
        ""NRC"": ""06833"",
        ""Cupo"": 20,
        ""CupoDisponible"": 1,
        ""Periodo"": ""04/02/14 - 13/06/14"",
        ""LugaresHoras"": [
          {
            ""HoraInicio"": {
              ""ticks"": 288000000000
            },
            ""HoraFin"": {
              ""ticks"": 357000000000
            },
            ""Dias"": [
              6
            ],
            ""Edificio"": ""DUCT2"",
            ""Aula"": ""LC10""
          }
        ]
      },
      {
        ""CentroUniversitario"": ""D"",
        ""Maestro"": ""0800-0955"",
        ""Seccion"": ""D11"",
        ""NRC"": ""06834"",
        ""Cupo"": 23,
        ""CupoDisponible"": 1,
        ""Periodo"": ""04/02/14 - 13/06/14"",
        ""LugaresHoras"": [
          {
            ""HoraInicio"": {
              ""ticks"": 288000000000
            },
            ""HoraFin"": {
              ""ticks"": 357000000000
            },
            ""Dias"": [
              6
            ],
            ""Edificio"": ""DUCT1"",
            ""Aula"": ""LC04""
          }
        ]
      },
      {
        ""CentroUniversitario"": ""D"",
        ""Maestro"": ""0900-1055"",
        ""Seccion"": ""D14"",
        ""NRC"": ""06837"",
        ""Cupo"": 23,
        ""CupoDisponible"": 1,
        ""Periodo"": ""04/02/14 - 13/06/14"",
        ""LugaresHoras"": [
          {
            ""HoraInicio"": {
              ""ticks"": 324000000000
            },
            ""HoraFin"": {
              ""ticks"": 393000000000
            },
            ""Dias"": [
              2
            ],
            ""Edificio"": ""DUCT1"",
            ""Aula"": ""LC10""
          }
        ]
      },
      {
        ""CentroUniversitario"": ""D"",
        ""Maestro"": ""0900-1055"",
        ""Seccion"": ""D15"",
        ""NRC"": ""06838"",
        ""Cupo"": 0,
        ""CupoDisponible"": 0,
        ""Periodo"": ""04/02/14 - 13/06/14"",
        ""LugaresHoras"": [
          {
            ""HoraInicio"": {
              ""ticks"": 324000000000
            },
            ""HoraFin"": {
              ""ticks"": 393000000000
            },
            ""Dias"": [
              3
            ],
            ""Edificio"": ""DUCT2"",
            ""Aula"": ""LC07""
          }
        ]
      },
      {
        ""CentroUniversitario"": ""D"",
        ""Maestro"": ""1700-1855"",
        ""Seccion"": ""D33"",
        ""NRC"": ""06854"",
        ""Cupo"": 20,
        ""CupoDisponible"": 2,
        ""Periodo"": ""04/02/14 - 13/06/14"",
        ""LugaresHoras"": [
          {
            ""HoraInicio"": {
              ""ticks"": 612000000000
            },
            ""HoraFin"": {
              ""ticks"": 681000000000
            },
            ""Dias"": [
              2
            ],
            ""Edificio"": ""DUCT1"",
            ""Aula"": ""LC08""
          }
        ]
      },
      {
        ""CentroUniversitario"": ""D"",
        ""Maestro"": ""1100-1255"",
        ""Seccion"": ""D34"",
        ""NRC"": ""06855"",
        ""Cupo"": 21,
        ""CupoDisponible"": 0,
        ""Periodo"": ""04/02/14 - 13/06/14"",
        ""LugaresHoras"": [
          {
            ""HoraInicio"": {
              ""ticks"": 396000000000
            },
            ""HoraFin"": {
              ""ticks"": 465000000000
            },
            ""Dias"": [
              4
            ],
            ""Edificio"": ""DUCT1"",
            ""Aula"": ""LC04""
          }
        ]
      },
      {
        ""CentroUniversitario"": ""D"",
        ""Maestro"": ""1100-1255"",
        ""Seccion"": ""D36"",
        ""NRC"": ""06857"",
        ""Cupo"": 22,
        ""CupoDisponible"": 1,
        ""Periodo"": ""04/02/14 - 13/06/14"",
        ""LugaresHoras"": [
          {
            ""HoraInicio"": {
              ""ticks"": 396000000000
            },
            ""HoraFin"": {
              ""ticks"": 465000000000
            },
            ""Dias"": [
              5
            ],
            ""Edificio"": ""DEDR"",
            ""Aula"": ""LC06""
          }
        ]
      },
      {
        ""CentroUniversitario"": ""D"",
        ""Maestro"": ""1700-1855"",
        ""Seccion"": ""D38"",
        ""NRC"": ""06859"",
        ""Cupo"": 20,
        ""CupoDisponible"": 3,
        ""Periodo"": ""04/02/14 - 13/06/14"",
        ""LugaresHoras"": [
          {
            ""HoraInicio"": {
              ""ticks"": 612000000000
            },
            ""HoraFin"": {
              ""ticks"": 681000000000
            },
            ""Dias"": [
              4
            ],
            ""Edificio"": ""DUCT1"",
            ""Aula"": ""LC08""
          }
        ]
      },
      {
        ""CentroUniversitario"": ""D"",
        ""Maestro"": ""1700-1855"",
        ""Seccion"": ""D53"",
        ""NRC"": ""06874"",
        ""Cupo"": 0,
        ""CupoDisponible"": 0,
        ""Periodo"": ""04/02/14 - 13/06/14"",
        ""LugaresHoras"": [
          {
            ""HoraInicio"": {
              ""ticks"": 612000000000
            },
            ""HoraFin"": {
              ""ticks"": 681000000000
            },
            ""Dias"": [
              5
            ],
            ""Edificio"": ""DUCT2"",
            ""Aula"": ""LC09""
          }
        ]
      },
      {
        ""CentroUniversitario"": ""D"",
        ""Maestro"": ""1700-1855"",
        ""Seccion"": ""D57"",
        ""NRC"": ""06878"",
        ""Cupo"": 20,
        ""CupoDisponible"": 1,
        ""Periodo"": ""04/02/14 - 13/06/14"",
        ""LugaresHoras"": [
          {
            ""HoraInicio"": {
              ""ticks"": 612000000000
            },
            ""HoraFin"": {
              ""ticks"": 681000000000
            },
            ""Dias"": [
              3
            ],
            ""Edificio"": ""DUCT1"",
            ""Aula"": ""LC10""
          }
        ]
      },
      {
        ""CentroUniversitario"": ""D"",
        ""Maestro"": ""1100-1255"",
        ""Seccion"": ""D63"",
        ""NRC"": ""06884"",
        ""Cupo"": 22,
        ""CupoDisponible"": 1,
        ""Periodo"": ""04/02/14 - 13/06/14"",
        ""LugaresHoras"": [
          {
            ""HoraInicio"": {
              ""ticks"": 396000000000
            },
            ""HoraFin"": {
              ""ticks"": 465000000000
            },
            ""Dias"": [
              1
            ],
            ""Edificio"": ""DUCT1"",
            ""Aula"": ""LC04""
          }
        ]
      },
      {
        ""CentroUniversitario"": ""H"",
        ""Maestro"": ""1100-1255"",
        ""Seccion"": ""H10"",
        ""NRC"": ""20235"",
        ""Cupo"": 50,
        ""CupoDisponible"": 9,
        ""Periodo"": ""04/02/14 - 13/06/14"",
        ""LugaresHoras"": [
          {
            ""HoraInicio"": {
              ""ticks"": 396000000000
            },
            ""HoraFin"": {
              ""ticks"": 465000000000
            },
            ""Dias"": [
              4
            ],
            ""Edificio"": "" "",
            ""Aula"": "" ""
          }
        ]
      },
      {
        ""CentroUniversitario"": ""I"",
        ""Maestro"": ""1900-2055"",
        ""Seccion"": ""I01"",
        ""NRC"": ""21717"",
        ""Cupo"": 45,
        ""CupoDisponible"": 8,
        ""Periodo"": ""04/02/14 - 10/04/14"",
        ""LugaresHoras"": [
          {
            ""HoraInicio"": {
              ""ticks"": 684000000000
            },
            ""HoraFin"": {
              ""ticks"": 753000000000
            },
            ""Dias"": [
              2,
              4
            ],
            ""Edificio"": ""IEDF"",
            ""Aula"": ""0003""
          }
        ]
      }
    ]
  },
  {
    ""Clave"": ""CC102"",
    ""Calendario"": ""201410"",
    ""Nombre"": ""INTRODUCCION A LA PROGRAMACION"",
    ""Creditos"": 8,
    ""Grupos"": [
      {
        ""CentroUniversitario"": ""D"",
        ""Maestro"": ""1000-1255"",
        ""Seccion"": ""D01"",
        ""NRC"": ""06896"",
        ""Cupo"": 41,
        ""CupoDisponible"": 2,
        ""Periodo"": ""04/02/14 - 13/06/14"",
        ""LugaresHoras"": [
          {
            ""HoraInicio"": {
              ""ticks"": 360000000000
            },
            ""HoraFin"": {
              ""ticks"": 465000000000
            },
            ""Dias"": [
              6
            ],
            ""Edificio"": ""DEDX"",
            ""Aula"": ""A016""
          }
        ]
      },
      {
        ""CentroUniversitario"": ""D"",
        ""Maestro"": ""0700-08550700-0755 "",
        ""Seccion"": ""D02"",
        ""NRC"": ""06897"",
        ""Cupo"": 45,
        ""CupoDisponible"": 7,
        ""Periodo"": ""04/02/14 - 13/06/14"",
        ""LugaresHoras"": [
          {
            ""HoraInicio"": {
              ""ticks"": 252000000000
            },
            ""HoraFin"": {
              ""ticks"": 321000000000
            },
            ""Dias"": [
              1
            ],
            ""Edificio"": ""DEDV"",
            ""Aula"": ""A001""
          },
          {
            ""HoraInicio"": {
              ""ticks"": 252000000000
            },
            ""HoraFin"": {
              ""ticks"": 285000000000
            },
            ""Dias"": [
              3
            ],
            ""Edificio"": ""DEDV"",
            ""Aula"": ""A001""
          }
        ]
      },
      {
        ""CentroUniversitario"": ""D"",
        ""Maestro"": ""1300-14551400-1455 "",
        ""Seccion"": ""D03"",
        ""NRC"": ""06898"",
        ""Cupo"": 40,
        ""CupoDisponible"": 0,
        ""Periodo"": ""04/02/14 - 13/06/14"",
        ""LugaresHoras"": [
          {
            ""HoraInicio"": {
              ""ticks"": 468000000000
            },
            ""HoraFin"": {
              ""ticks"": 537000000000
            },
            ""Dias"": [
              5
            ],
            ""Edificio"": ""DEDI"",
            ""Aula"": ""A005""
          },
          {
            ""HoraInicio"": {
              ""ticks"": 504000000000
            },
            ""HoraFin"": {
              ""ticks"": 537000000000
            },
            ""Dias"": [
              3
            ],
            ""Edificio"": ""DEDI"",
            ""Aula"": ""A005""
          }
        ]
      },
      {
        ""CentroUniversitario"": ""D"",
        ""Maestro"": ""0700-08550800-0855 "",
        ""Seccion"": ""D04"",
        ""NRC"": ""06899"",
        ""Cupo"": 42,
        ""CupoDisponible"": 4,
        ""Periodo"": ""04/02/14 - 13/06/14"",
        ""LugaresHoras"": [
          {
            ""HoraInicio"": {
              ""ticks"": 252000000000
            },
            ""HoraFin"": {
              ""ticks"": 321000000000
            },
            ""Dias"": [
              5
            ],
            ""Edificio"": ""DEDT"",
            ""Aula"": ""A019""
          },
          {
            ""HoraInicio"": {
              ""ticks"": 288000000000
            },
            ""HoraFin"": {
              ""ticks"": 321000000000
            },
            ""Dias"": [
              3
            ],
            ""Edificio"": ""DEDT"",
            ""Aula"": ""A017""
          }
        ]
      },
      {
        ""CentroUniversitario"": ""D"",
        ""Maestro"": ""1100-12551100-1155 "",
        ""Seccion"": ""D05"",
        ""NRC"": ""06900"",
        ""Cupo"": 40,
        ""CupoDisponible"": 3,
        ""Periodo"": ""04/02/14 - 13/06/14"",
        ""LugaresHoras"": [
          {
            ""HoraInicio"": {
              ""ticks"": 396000000000
            },
            ""HoraFin"": {
              ""ticks"": 465000000000
            },
            ""Dias"": [
              1
            ],
            ""Edificio"": ""DEDX"",
            ""Aula"": ""A014""
          },
          {
            ""HoraInicio"": {
              ""ticks"": 396000000000
            },
            ""HoraFin"": {
              ""ticks"": 429000000000
            },
            ""Dias"": [
              3
            ],
            ""Edificio"": ""DEDU"",
            ""Aula"": ""A008""
          }
        ]
      },
      {
        ""CentroUniversitario"": ""D"",
        ""Maestro"": ""0900-10550900-0955 "",
        ""Seccion"": ""D08"",
        ""NRC"": ""06903"",
        ""Cupo"": 44,
        ""CupoDisponible"": 2,
        ""Periodo"": ""04/02/14 - 13/06/14"",
        ""LugaresHoras"": [
          {
            ""HoraInicio"": {
              ""ticks"": 324000000000
            },
            ""HoraFin"": {
              ""ticks"": 393000000000
            },
            ""Dias"": [
              1
            ],
            ""Edificio"": ""DEDX"",
            ""Aula"": ""A003""
          },
          {
            ""HoraInicio"": {
              ""ticks"": 324000000000
            },
            ""HoraFin"": {
              ""ticks"": 357000000000
            },
            ""Dias"": [
              3
            ],
            ""Edificio"": ""DEDX"",
            ""Aula"": ""A003""
          }
        ]
      },
      {
        ""CentroUniversitario"": ""D"",
        ""Maestro"": ""1900-20551900-1955 "",
        ""Seccion"": ""D12"",
        ""NRC"": ""06907"",
        ""Cupo"": 40,
        ""CupoDisponible"": 1,
        ""Periodo"": ""04/02/14 - 13/06/14"",
        ""LugaresHoras"": [
          {
            ""HoraInicio"": {
              ""ticks"": 684000000000
            },
            ""HoraFin"": {
              ""ticks"": 753000000000
            },
            ""Dias"": [
              1
            ],
            ""Edificio"": ""DEDS"",
            ""Aula"": "" ""
          },
          {
            ""HoraInicio"": {
              ""ticks"": 684000000000
            },
            ""HoraFin"": {
              ""ticks"": 717000000000
            },
            ""Dias"": [
              3
            ],
            ""Edificio"": ""DEDS"",
            ""Aula"": "" ""
          }
        ]
      },
      {
        ""CentroUniversitario"": ""D"",
        ""Maestro"": ""1300-14551400-1455 "",
        ""Seccion"": ""D17"",
        ""NRC"": ""06912"",
        ""Cupo"": 40,
        ""CupoDisponible"": 9,
        ""Periodo"": ""04/02/14 - 13/06/14"",
        ""LugaresHoras"": [
          {
            ""HoraInicio"": {
              ""ticks"": 468000000000
            },
            ""HoraFin"": {
              ""ticks"": 537000000000
            },
            ""Dias"": [
              5
            ],
            ""Edificio"": ""DEDL"",
            ""Aula"": "" ""
          },
          {
            ""HoraInicio"": {
              ""ticks"": 504000000000
            },
            ""HoraFin"": {
              ""ticks"": 537000000000
            },
            ""Dias"": [
              3
            ],
            ""Edificio"": ""DEDL"",
            ""Aula"": "" ""
          }
        ]
      },
      {
        ""CentroUniversitario"": ""D"",
        ""Maestro"": ""1100-12551100-1155 "",
        ""Seccion"": ""D18"",
        ""NRC"": ""06913"",
        ""Cupo"": 40,
        ""CupoDisponible"": 4,
        ""Periodo"": ""04/02/14 - 13/06/14"",
        ""LugaresHoras"": [
          {
            ""HoraInicio"": {
              ""ticks"": 396000000000
            },
            ""HoraFin"": {
              ""ticks"": 465000000000
            },
            ""Dias"": [
              1
            ],
            ""Edificio"": ""DEDT"",
            ""Aula"": ""A004""
          },
          {
            ""HoraInicio"": {
              ""ticks"": 396000000000
            },
            ""HoraFin"": {
              ""ticks"": 429000000000
            },
            ""Dias"": [
              3
            ],
            ""Edificio"": ""DEDT"",
            ""Aula"": ""A004""
          }
        ]
      },
      {
        ""CentroUniversitario"": ""D"",
        ""Maestro"": ""0900-10550900-0955 "",
        ""Seccion"": ""D22"",
        ""NRC"": ""06917"",
        ""Cupo"": 40,
        ""CupoDisponible"": 4,
        ""Periodo"": ""04/02/14 - 13/06/14"",
        ""LugaresHoras"": [
          {
            ""HoraInicio"": {
              ""ticks"": 324000000000
            },
            ""HoraFin"": {
              ""ticks"": 393000000000
            },
            ""Dias"": [
              2
            ],
            ""Edificio"": ""DEDF"",
            ""Aula"": ""A003""
          },
          {
            ""HoraInicio"": {
              ""ticks"": 324000000000
            },
            ""HoraFin"": {
              ""ticks"": 357000000000
            },
            ""Dias"": [
              4
            ],
            ""Edificio"": ""DEDF"",
            ""Aula"": ""A003""
          }
        ]
      },
      {
        ""CentroUniversitario"": ""D"",
        ""Maestro"": ""1900-2155"",
        ""Seccion"": ""D23"",
        ""NRC"": ""06918"",
        ""Cupo"": 40,
        ""CupoDisponible"": 8,
        ""Periodo"": ""04/02/14 - 13/06/14"",
        ""LugaresHoras"": [
          {
            ""HoraInicio"": {
              ""ticks"": 684000000000
            },
            ""HoraFin"": {
              ""ticks"": 789000000000
            },
            ""Dias"": [
              5
            ],
            ""Edificio"": ""DEDQ"",
            ""Aula"": ""A028""
          }
        ]
      },
      {
        ""CentroUniversitario"": ""D"",
        ""Maestro"": ""1300-14551300-1355 "",
        ""Seccion"": ""D24"",
        ""NRC"": ""06919"",
        ""Cupo"": 40,
        ""CupoDisponible"": 0,
        ""Periodo"": ""04/02/14 - 13/06/14"",
        ""LugaresHoras"": [
          {
            ""HoraInicio"": {
              ""ticks"": 468000000000
            },
            ""HoraFin"": {
              ""ticks"": 537000000000
            },
            ""Dias"": [
              1
            ],
            ""Edificio"": ""DEDQ"",
            ""Aula"": ""A029""
          },
          {
            ""HoraInicio"": {
              ""ticks"": 468000000000
            },
            ""HoraFin"": {
              ""ticks"": 501000000000
            },
            ""Dias"": [
              3
            ],
            ""Edificio"": ""DEDQ"",
            ""Aula"": ""A029""
          }
        ]
      },
      {
        ""CentroUniversitario"": ""D"",
        ""Maestro"": ""1900-20551900-1955 "",
        ""Seccion"": ""D27"",
        ""NRC"": ""06922"",
        ""Cupo"": 41,
        ""CupoDisponible"": 1,
        ""Periodo"": ""04/02/14 - 13/06/14"",
        ""LugaresHoras"": [
          {
            ""HoraInicio"": {
              ""ticks"": 684000000000
            },
            ""HoraFin"": {
              ""ticks"": 753000000000
            },
            ""Dias"": [
              1
            ],
            ""Edificio"": ""DEDX"",
            ""Aula"": ""A019""
          },
          {
            ""HoraInicio"": {
              ""ticks"": 684000000000
            },
            ""HoraFin"": {
              ""ticks"": 717000000000
            },
            ""Dias"": [
              3
            ],
            ""Edificio"": ""DEDX"",
            ""Aula"": ""A019""
          }
        ]
      },
      {
        ""CentroUniversitario"": ""D"",
        ""Maestro"": ""1100-12551100-1155 "",
        ""Seccion"": ""D28"",
        ""NRC"": ""06923"",
        ""Cupo"": 40,
        ""CupoDisponible"": 1,
        ""Periodo"": ""04/02/14 - 13/06/14"",
        ""LugaresHoras"": [
          {
            ""HoraInicio"": {
              ""ticks"": 396000000000
            },
            ""HoraFin"": {
              ""ticks"": 465000000000
            },
            ""Dias"": [
              1
            ],
            ""Edificio"": ""DEDQ"",
            ""Aula"": ""A028""
          },
          {
            ""HoraInicio"": {
              ""ticks"": 396000000000
            },
            ""HoraFin"": {
              ""ticks"": 429000000000
            },
            ""Dias"": [
              3
            ],
            ""Edificio"": ""DEDQ"",
            ""Aula"": ""A028""
          }
        ]
      },
      {
        ""CentroUniversitario"": ""D"",
        ""Maestro"": ""1500-16551600-1655 "",
        ""Seccion"": ""D29"",
        ""NRC"": ""06924"",
        ""Cupo"": 40,
        ""CupoDisponible"": 1,
        ""Periodo"": ""04/02/14 - 13/06/14"",
        ""LugaresHoras"": [
          {
            ""HoraInicio"": {
              ""ticks"": 540000000000
            },
            ""HoraFin"": {
              ""ticks"": 609000000000
            },
            ""Dias"": [
              2
            ],
            ""Edificio"": ""DEDQ"",
            ""Aula"": ""A027""
          },
          {
            ""HoraInicio"": {
              ""ticks"": 576000000000
            },
            ""HoraFin"": {
              ""ticks"": 609000000000
            },
            ""Dias"": [
              4
            ],
            ""Edificio"": ""DEDQ"",
            ""Aula"": ""A027""
          }
        ]
      },
      {
        ""CentroUniversitario"": ""D"",
        ""Maestro"": ""1500-16551500-1555 "",
        ""Seccion"": ""D30"",
        ""NRC"": ""06925"",
        ""Cupo"": 42,
        ""CupoDisponible"": 2,
        ""Periodo"": ""04/02/14 - 13/06/14"",
        ""LugaresHoras"": [
          {
            ""HoraInicio"": {
              ""ticks"": 540000000000
            },
            ""HoraFin"": {
              ""ticks"": 609000000000
            },
            ""Dias"": [
              1
            ],
            ""Edificio"": ""DEDT"",
            ""Aula"": ""A003""
          },
          {
            ""HoraInicio"": {
              ""ticks"": 540000000000
            },
            ""HoraFin"": {
              ""ticks"": 573000000000
            },
            ""Dias"": [
              3
            ],
            ""Edificio"": ""DEDT"",
            ""Aula"": ""A003""
          }
        ]
      },
      {
        ""CentroUniversitario"": ""D"",
        ""Maestro"": ""1900-20551900-1955 "",
        ""Seccion"": ""D33"",
        ""NRC"": ""06928"",
        ""Cupo"": 40,
        ""CupoDisponible"": 5,
        ""Periodo"": ""04/02/14 - 13/06/14"",
        ""LugaresHoras"": [
          {
            ""HoraInicio"": {
              ""ticks"": 684000000000
            },
            ""HoraFin"": {
              ""ticks"": 753000000000
            },
            ""Dias"": [
              1
            ],
            ""Edificio"": ""DEDF"",
            ""Aula"": ""A003""
          },
          {
            ""HoraInicio"": {
              ""ticks"": 684000000000
            },
            ""HoraFin"": {
              ""ticks"": 717000000000
            },
            ""Dias"": [
              3
            ],
            ""Edificio"": ""DEDF"",
            ""Aula"": ""A003""
          }
        ]
      },
      {
        ""CentroUniversitario"": ""D"",
        ""Maestro"": ""1100-12551100-1155 "",
        ""Seccion"": ""D34"",
        ""NRC"": ""06929"",
        ""Cupo"": 46,
        ""CupoDisponible"": 2,
        ""Periodo"": ""04/02/14 - 13/06/14"",
        ""LugaresHoras"": [
          {
            ""HoraInicio"": {
              ""ticks"": 396000000000
            },
            ""HoraFin"": {
              ""ticks"": 465000000000
            },
            ""Dias"": [
              1
            ],
            ""Edificio"": ""DEDK"",
            ""Aula"": ""D001""
          },
          {
            ""HoraInicio"": {
              ""ticks"": 396000000000
            },
            ""HoraFin"": {
              ""ticks"": 429000000000
            },
            ""Dias"": [
              5
            ],
            ""Edificio"": ""DEDK"",
            ""Aula"": ""D001""
          }
        ]
      },
      {
        ""CentroUniversitario"": ""H"",
        ""Maestro"": ""0700-08590900-0955 "",
        ""Seccion"": ""H01"",
        ""NRC"": ""20236"",
        ""Cupo"": 40,
        ""CupoDisponible"": 19,
        ""Periodo"": ""04/02/14 - 13/06/14"",
        ""LugaresHoras"": [
          {
            ""HoraInicio"": {
              ""ticks"": 252000000000
            },
            ""HoraFin"": {
              ""ticks"": 323400000000
            },
            ""Dias"": [
              3
            ],
            ""Edificio"": ""HEDI"",
            ""Aula"": ""0103""
          },
          {
            ""HoraInicio"": {
              ""ticks"": 324000000000
            },
            ""HoraFin"": {
              ""ticks"": 357000000000
            },
            ""Dias"": [
              3
            ],
            ""Edificio"": ""HEDC"",
            ""Aula"": ""0009""
          }
        ]
      },
      {
        ""CentroUniversitario"": ""H"",
        ""Maestro"": ""1100-1355"",
        ""Seccion"": ""H02"",
        ""NRC"": ""20237"",
        ""Cupo"": 1,
        ""CupoDisponible"": 0,
        ""Periodo"": ""04/02/14 - 13/06/14"",
        ""LugaresHoras"": [
          {
            ""HoraInicio"": {
              ""ticks"": 396000000000
            },
            ""HoraFin"": {
              ""ticks"": 501000000000
            },
            ""Dias"": [
              5
            ],
            ""Edificio"": ""HEDC"",
            ""Aula"": ""0002""
          }
        ]
      },
      {
        ""CentroUniversitario"": ""I"",
        ""Maestro"": ""1700-1855"",
        ""Seccion"": ""I02"",
        ""NRC"": ""21720"",
        ""Cupo"": 40,
        ""CupoDisponible"": 3,
        ""Periodo"": ""04/02/14 - 29/05/14"",
        ""LugaresHoras"": [
          {
            ""HoraInicio"": {
              ""ticks"": 612000000000
            },
            ""HoraFin"": {
              ""ticks"": 681000000000
            },
            ""Dias"": [
              2,
              4
            ],
            ""Edificio"": ""IEDF"",
            ""Aula"": ""0003""
          }
        ]
      }
    ]
  },
  {
    ""Clave"": ""CC103"",
    ""Calendario"": ""201410"",
    ""Nombre"": ""TALLER DE PROGRAMACION ESTRUCTURADA"",
    ""Creditos"": 4,
    ""Grupos"": [
      {
        ""CentroUniversitario"": ""D"",
        ""Maestro"": ""0900-10550900-0955 "",
        ""Seccion"": ""D01"",
        ""NRC"": ""06934"",
        ""Cupo"": 25,
        ""CupoDisponible"": 0,
        ""Periodo"": ""04/02/14 - 13/06/14"",
        ""LugaresHoras"": [
          {
            ""HoraInicio"": {
              ""ticks"": 324000000000
            },
            ""HoraFin"": {
              ""ticks"": 393000000000
            },
            ""Dias"": [
              1
            ],
            ""Edificio"": ""DUCT2"",
            ""Aula"": ""LC05""
          },
          {
            ""HoraInicio"": {
              ""ticks"": 324000000000
            },
            ""HoraFin"": {
              ""ticks"": 357000000000
            },
            ""Dias"": [
              3
            ],
            ""Edificio"": ""DUCT2"",
            ""Aula"": ""LC05""
          }
        ]
      },
      {
        ""CentroUniversitario"": ""D"",
        ""Maestro"": ""1200-1455"",
        ""Seccion"": ""D02"",
        ""NRC"": ""06935"",
        ""Cupo"": 24,
        ""CupoDisponible"": 1,
        ""Periodo"": ""04/02/14 - 13/06/14"",
        ""LugaresHoras"": [
          {
            ""HoraInicio"": {
              ""ticks"": 432000000000
            },
            ""HoraFin"": {
              ""ticks"": 537000000000
            },
            ""Dias"": [
              4
            ],
            ""Edificio"": ""DUCT2"",
            ""Aula"": ""LC10""
          }
        ]
      },
      {
        ""CentroUniversitario"": ""D"",
        ""Maestro"": ""1500-16551500-1500 "",
        ""Seccion"": ""D03"",
        ""NRC"": ""06936"",
        ""Cupo"": 24,
        ""CupoDisponible"": 2,
        ""Periodo"": ""04/02/14 - 13/06/14"",
        ""LugaresHoras"": [
          {
            ""HoraInicio"": {
              ""ticks"": 540000000000
            },
            ""HoraFin"": {
              ""ticks"": 609000000000
            },
            ""Dias"": [
              1
            ],
            ""Edificio"": ""DUCT2"",
            ""Aula"": ""LC06""
          },
          {
            ""HoraInicio"": {
              ""ticks"": 540000000000
            },
            ""HoraFin"": {
              ""ticks"": 540000000000
            },
            ""Dias"": [
              3
            ],
            ""Edificio"": ""DUCT2"",
            ""Aula"": ""LC06""
          }
        ]
      },
      {
        ""CentroUniversitario"": ""D"",
        ""Maestro"": ""1100-1355"",
        ""Seccion"": ""D04"",
        ""NRC"": ""06937"",
        ""Cupo"": 24,
        ""CupoDisponible"": 5,
        ""Periodo"": ""04/02/14 - 13/06/14"",
        ""LugaresHoras"": [
          {
            ""HoraInicio"": {
              ""ticks"": 396000000000
            },
            ""HoraFin"": {
              ""ticks"": 501000000000
            },
            ""Dias"": [
              5
            ],
            ""Edificio"": ""DUCT2"",
            ""Aula"": ""LC05""
          }
        ]
      },
      {
        ""CentroUniversitario"": ""D"",
        ""Maestro"": ""1700-18551700-1755 "",
        ""Seccion"": ""D06"",
        ""NRC"": ""06939"",
        ""Cupo"": 25,
        ""CupoDisponible"": 2,
        ""Periodo"": ""04/02/14 - 13/06/14"",
        ""LugaresHoras"": [
          {
            ""HoraInicio"": {
              ""ticks"": 612000000000
            },
            ""HoraFin"": {
              ""ticks"": 681000000000
            },
            ""Dias"": [
              2
            ],
            ""Edificio"": ""DUCT2"",
            ""Aula"": ""LC05""
          },
          {
            ""HoraInicio"": {
              ""ticks"": 612000000000
            },
            ""HoraFin"": {
              ""ticks"": 645000000000
            },
            ""Dias"": [
              4
            ],
            ""Edificio"": ""DUCT2"",
            ""Aula"": ""LC05""
          }
        ]
      },
      {
        ""CentroUniversitario"": ""D"",
        ""Maestro"": ""1100-12551200-1255 "",
        ""Seccion"": ""D07"",
        ""NRC"": ""06940"",
        ""Cupo"": 24,
        ""CupoDisponible"": 1,
        ""Periodo"": ""04/02/14 - 13/06/14"",
        ""LugaresHoras"": [
          {
            ""HoraInicio"": {
              ""ticks"": 396000000000
            },
            ""HoraFin"": {
              ""ticks"": 465000000000
            },
            ""Dias"": [
              5
            ],
            ""Edificio"": ""DUCT1"",
            ""Aula"": ""LC04""
          },
          {
            ""HoraInicio"": {
              ""ticks"": 432000000000
            },
            ""HoraFin"": {
              ""ticks"": 465000000000
            },
            ""Dias"": [
              3
            ],
            ""Edificio"": ""DUCT1"",
            ""Aula"": ""LC04""
          }
        ]
      },
      {
        ""CentroUniversitario"": ""D"",
        ""Maestro"": ""1300-1555"",
        ""Seccion"": ""D11"",
        ""NRC"": ""06944"",
        ""Cupo"": 26,
        ""CupoDisponible"": 1,
        ""Periodo"": ""04/02/14 - 13/06/14"",
        ""LugaresHoras"": [
          {
            ""HoraInicio"": {
              ""ticks"": 468000000000
            },
            ""HoraFin"": {
              ""ticks"": 573000000000
            },
            ""Dias"": [
              6
            ],
            ""Edificio"": ""DUCT1"",
            ""Aula"": ""LC04""
          }
        ]
      },
      {
        ""CentroUniversitario"": ""D"",
        ""Maestro"": ""0700-0955"",
        ""Seccion"": ""D13"",
        ""NRC"": ""06946"",
        ""Cupo"": 24,
        ""CupoDisponible"": 5,
        ""Periodo"": ""04/02/14 - 13/06/14"",
        ""LugaresHoras"": [
          {
            ""HoraInicio"": {
              ""ticks"": 252000000000
            },
            ""HoraFin"": {
              ""ticks"": 357000000000
            },
            ""Dias"": [
              6
            ],
            ""Edificio"": ""DUCT1"",
            ""Aula"": ""LC10""
          }
        ]
      },
      {
        ""CentroUniversitario"": ""D"",
        ""Maestro"": ""1100-12551100-1155 "",
        ""Seccion"": ""D26"",
        ""NRC"": ""06959"",
        ""Cupo"": 24,
        ""CupoDisponible"": 7,
        ""Periodo"": ""04/02/14 - 13/06/14"",
        ""LugaresHoras"": [
          {
            ""HoraInicio"": {
              ""ticks"": 396000000000
            },
            ""HoraFin"": {
              ""ticks"": 465000000000
            },
            ""Dias"": [
              1
            ],
            ""Edificio"": ""DUCT2"",
            ""Aula"": ""LC10""
          },
          {
            ""HoraInicio"": {
              ""ticks"": 396000000000
            },
            ""HoraFin"": {
              ""ticks"": 429000000000
            },
            ""Dias"": [
              3
            ],
            ""Edificio"": ""DUCT2"",
            ""Aula"": ""LC10""
          }
        ]
      },
      {
        ""CentroUniversitario"": ""D"",
        ""Maestro"": ""1100-11551100-1255 "",
        ""Seccion"": ""D29"",
        ""NRC"": ""06962"",
        ""Cupo"": 24,
        ""CupoDisponible"": 0,
        ""Periodo"": ""04/02/14 - 13/06/14"",
        ""LugaresHoras"": [
          {
            ""HoraInicio"": {
              ""ticks"": 396000000000
            },
            ""HoraFin"": {
              ""ticks"": 429000000000
            },
            ""Dias"": [
              2
            ],
            ""Edificio"": ""DUCT2"",
            ""Aula"": ""LC08""
          },
          {
            ""HoraInicio"": {
              ""ticks"": 396000000000
            },
            ""HoraFin"": {
              ""ticks"": 465000000000
            },
            ""Dias"": [
              4
            ],
            ""Edificio"": ""DUCT2"",
            ""Aula"": ""LC06""
          }
        ]
      },
      {
        ""CentroUniversitario"": ""D"",
        ""Maestro"": ""1300-14551400-1455 "",
        ""Seccion"": ""D30"",
        ""NRC"": ""06963"",
        ""Cupo"": 24,
        ""CupoDisponible"": 2,
        ""Periodo"": ""04/02/14 - 13/06/14"",
        ""LugaresHoras"": [
          {
            ""HoraInicio"": {
              ""ticks"": 468000000000
            },
            ""HoraFin"": {
              ""ticks"": 537000000000
            },
            ""Dias"": [
              5
            ],
            ""Edificio"": ""DUCT2"",
            ""Aula"": ""LC10""
          },
          {
            ""HoraInicio"": {
              ""ticks"": 504000000000
            },
            ""HoraFin"": {
              ""ticks"": 537000000000
            },
            ""Dias"": [
              3
            ],
            ""Edificio"": ""DEDW"",
            ""Aula"": ""A006""
          }
        ]
      },
      {
        ""CentroUniversitario"": ""D"",
        ""Maestro"": ""1300-14551300-1355 "",
        ""Seccion"": ""D32"",
        ""NRC"": ""06965"",
        ""Cupo"": 24,
        ""CupoDisponible"": 1,
        ""Periodo"": ""04/02/14 - 13/06/14"",
        ""LugaresHoras"": [
          {
            ""HoraInicio"": {
              ""ticks"": 468000000000
            },
            ""HoraFin"": {
              ""ticks"": 537000000000
            },
            ""Dias"": [
              2
            ],
            ""Edificio"": ""DUCT1"",
            ""Aula"": ""LC08""
          },
          {
            ""HoraInicio"": {
              ""ticks"": 468000000000
            },
            ""HoraFin"": {
              ""ticks"": 501000000000
            },
            ""Dias"": [
              4
            ],
            ""Edificio"": ""DEDW"",
            ""Aula"": ""A004""
          }
        ]
      },
      {
        ""CentroUniversitario"": ""D"",
        ""Maestro"": ""1100-12551200-1255 "",
        ""Seccion"": ""D34"",
        ""NRC"": ""06967"",
        ""Cupo"": 25,
        ""CupoDisponible"": 2,
        ""Periodo"": ""04/02/14 - 13/06/14"",
        ""LugaresHoras"": [
          {
            ""HoraInicio"": {
              ""ticks"": 396000000000
            },
            ""HoraFin"": {
              ""ticks"": 465000000000
            },
            ""Dias"": [
              5
            ],
            ""Edificio"": ""DUCT1"",
            ""Aula"": ""LC09""
          },
          {
            ""HoraInicio"": {
              ""ticks"": 432000000000
            },
            ""HoraFin"": {
              ""ticks"": 465000000000
            },
            ""Dias"": [
              3
            ],
            ""Edificio"": ""DUCT1"",
            ""Aula"": ""LC09""
          }
        ]
      },
      {
        ""CentroUniversitario"": ""D"",
        ""Maestro"": ""1500-16551500-1555 "",
        ""Seccion"": ""D35"",
        ""NRC"": ""06968"",
        ""Cupo"": 23,
        ""CupoDisponible"": 1,
        ""Periodo"": ""04/02/14 - 13/06/14"",
        ""LugaresHoras"": [
          {
            ""HoraInicio"": {
              ""ticks"": 540000000000
            },
            ""HoraFin"": {
              ""ticks"": 609000000000
            },
            ""Dias"": [
              2
            ],
            ""Edificio"": ""DUCT2"",
            ""Aula"": ""LC06""
          },
          {
            ""HoraInicio"": {
              ""ticks"": 540000000000
            },
            ""HoraFin"": {
              ""ticks"": 573000000000
            },
            ""Dias"": [
              4
            ],
            ""Edificio"": ""DUCT2"",
            ""Aula"": ""LC06""
          }
        ]
      },
      {
        ""CentroUniversitario"": ""D"",
        ""Maestro"": ""1500-16551500-1555 "",
        ""Seccion"": ""D36"",
        ""NRC"": ""06969"",
        ""Cupo"": 24,
        ""CupoDisponible"": 1,
        ""Periodo"": ""04/02/14 - 13/06/14"",
        ""LugaresHoras"": [
          {
            ""HoraInicio"": {
              ""ticks"": 540000000000
            },
            ""HoraFin"": {
              ""ticks"": 609000000000
            },
            ""Dias"": [
              1
            ],
            ""Edificio"": ""DUCT1"",
            ""Aula"": ""LC09""
          },
          {
            ""HoraInicio"": {
              ""ticks"": 540000000000
            },
            ""HoraFin"": {
              ""ticks"": 573000000000
            },
            ""Dias"": [
              3
            ],
            ""Edificio"": ""DUCT1"",
            ""Aula"": ""LC09""
          }
        ]
      },
      {
        ""CentroUniversitario"": ""D"",
        ""Maestro"": ""1100-12551100-1155 "",
        ""Seccion"": ""D39"",
        ""NRC"": ""06972"",
        ""Cupo"": 24,
        ""CupoDisponible"": 1,
        ""Periodo"": ""04/02/14 - 13/06/14"",
        ""LugaresHoras"": [
          {
            ""HoraInicio"": {
              ""ticks"": 396000000000
            },
            ""HoraFin"": {
              ""ticks"": 465000000000
            },
            ""Dias"": [
              1
            ],
            ""Edificio"": ""DUCT2"",
            ""Aula"": ""LC07""
          },
          {
            ""HoraInicio"": {
              ""ticks"": 396000000000
            },
            ""HoraFin"": {
              ""ticks"": 429000000000
            },
            ""Dias"": [
              3
            ],
            ""Edificio"": ""DUCT1"",
            ""Aula"": ""LC08""
          }
        ]
      },
      {
        ""CentroUniversitario"": ""D"",
        ""Maestro"": ""1100-12551200-1255 "",
        ""Seccion"": ""D41"",
        ""NRC"": ""06974"",
        ""Cupo"": 25,
        ""CupoDisponible"": 0,
        ""Periodo"": ""04/02/14 - 13/06/14"",
        ""LugaresHoras"": [
          {
            ""HoraInicio"": {
              ""ticks"": 396000000000
            },
            ""HoraFin"": {
              ""ticks"": 465000000000
            },
            ""Dias"": [
              3
            ],
            ""Edificio"": ""DUCT2"",
            ""Aula"": ""LC05""
          },
          {
            ""HoraInicio"": {
              ""ticks"": 432000000000
            },
            ""HoraFin"": {
              ""ticks"": 465000000000
            },
            ""Dias"": [
              5
            ],
            ""Edificio"": ""DEDW"",
            ""Aula"": ""A004""
          }
        ]
      },
      {
        ""CentroUniversitario"": ""D"",
        ""Maestro"": ""1900-20551900-1955 "",
        ""Seccion"": ""D42"",
        ""NRC"": ""06975"",
        ""Cupo"": 26,
        ""CupoDisponible"": 1,
        ""Periodo"": ""04/02/14 - 13/06/14"",
        ""LugaresHoras"": [
          {
            ""HoraInicio"": {
              ""ticks"": 684000000000
            },
            ""HoraFin"": {
              ""ticks"": 753000000000
            },
            ""Dias"": [
              2
            ],
            ""Edificio"": ""DUCT1"",
            ""Aula"": ""LC09""
          },
          {
            ""HoraInicio"": {
              ""ticks"": 684000000000
            },
            ""HoraFin"": {
              ""ticks"": 717000000000
            },
            ""Dias"": [
              4
            ],
            ""Edificio"": ""DEDT"",
            ""Aula"": ""A019""
          }
        ]
      },
      {
        ""CentroUniversitario"": ""D"",
        ""Maestro"": ""1300-14551400-1455 "",
        ""Seccion"": ""D46"",
        ""NRC"": ""06978"",
        ""Cupo"": 25,
        ""CupoDisponible"": 3,
        ""Periodo"": ""04/02/14 - 13/06/14"",
        ""LugaresHoras"": [
          {
            ""HoraInicio"": {
              ""ticks"": 468000000000
            },
            ""HoraFin"": {
              ""ticks"": 537000000000
            },
            ""Dias"": [
              5
            ],
            ""Edificio"": ""DUCT2"",
            ""Aula"": ""LC09""
          },
          {
            ""HoraInicio"": {
              ""ticks"": 504000000000
            },
            ""HoraFin"": {
              ""ticks"": 537000000000
            },
            ""Dias"": [
              3
            ],
            ""Edificio"": ""DEDI"",
            ""Aula"": ""A006""
          }
        ]
      },
      {
        ""CentroUniversitario"": ""D"",
        ""Maestro"": ""1100-12551100-1155 "",
        ""Seccion"": ""D49"",
        ""NRC"": ""06981"",
        ""Cupo"": 24,
        ""CupoDisponible"": 1,
        ""Periodo"": ""04/02/14 - 13/06/14"",
        ""LugaresHoras"": [
          {
            ""HoraInicio"": {
              ""ticks"": 396000000000
            },
            ""HoraFin"": {
              ""ticks"": 465000000000
            },
            ""Dias"": [
              2
            ],
            ""Edificio"": ""DUCT2"",
            ""Aula"": ""LC07""
          },
          {
            ""HoraInicio"": {
              ""ticks"": 396000000000
            },
            ""HoraFin"": {
              ""ticks"": 429000000000
            },
            ""Dias"": [
              4
            ],
            ""Edificio"": ""DUCT2"",
            ""Aula"": ""LC07""
          }
        ]
      },
      {
        ""CentroUniversitario"": ""D"",
        ""Maestro"": ""1300-14551300-1355 "",
        ""Seccion"": ""D51"",
        ""NRC"": ""06983"",
        ""Cupo"": 24,
        ""CupoDisponible"": 1,
        ""Periodo"": ""04/02/14 - 13/06/14"",
        ""LugaresHoras"": [
          {
            ""HoraInicio"": {
              ""ticks"": 468000000000
            },
            ""HoraFin"": {
              ""ticks"": 537000000000
            },
            ""Dias"": [
              1
            ],
            ""Edificio"": ""DUCT2"",
            ""Aula"": ""LC08""
          },
          {
            ""HoraInicio"": {
              ""ticks"": 468000000000
            },
            ""HoraFin"": {
              ""ticks"": 501000000000
            },
            ""Dias"": [
              3
            ],
            ""Edificio"": ""DUCT2"",
            ""Aula"": ""LC08""
          }
        ]
      },
      {
        ""CentroUniversitario"": ""D"",
        ""Maestro"": ""1700-18551700-1855 "",
        ""Seccion"": ""D52"",
        ""NRC"": ""06984"",
        ""Cupo"": 24,
        ""CupoDisponible"": 4,
        ""Periodo"": ""04/02/14 - 13/06/14"",
        ""LugaresHoras"": [
          {
            ""HoraInicio"": {
              ""ticks"": 612000000000
            },
            ""HoraFin"": {
              ""ticks"": 681000000000
            },
            ""Dias"": [
              1
            ],
            ""Edificio"": ""DUCT1"",
            ""Aula"": ""LC10""
          },
          {
            ""HoraInicio"": {
              ""ticks"": 612000000000
            },
            ""HoraFin"": {
              ""ticks"": 681000000000
            },
            ""Dias"": [
              3
            ],
            ""Edificio"": ""DEDX"",
            ""Aula"": ""A022""
          }
        ]
      },
      {
        ""CentroUniversitario"": ""D"",
        ""Maestro"": ""1700-18551800-1855 "",
        ""Seccion"": ""D54"",
        ""NRC"": ""06986"",
        ""Cupo"": 24,
        ""CupoDisponible"": 7,
        ""Periodo"": ""04/02/14 - 13/06/14"",
        ""LugaresHoras"": [
          {
            ""HoraInicio"": {
              ""ticks"": 612000000000
            },
            ""HoraFin"": {
              ""ticks"": 681000000000
            },
            ""Dias"": [
              5
            ],
            ""Edificio"": ""DUCT1"",
            ""Aula"": ""LC10""
          },
          {
            ""HoraInicio"": {
              ""ticks"": 648000000000
            },
            ""HoraFin"": {
              ""ticks"": 681000000000
            },
            ""Dias"": [
              3
            ],
            ""Edificio"": ""DUCT1"",
            ""Aula"": ""LC04""
          }
        ]
      },
      {
        ""CentroUniversitario"": ""D"",
        ""Maestro"": ""1900-19551900-2055 "",
        ""Seccion"": ""D55"",
        ""NRC"": ""06987"",
        ""Cupo"": 24,
        ""CupoDisponible"": 1,
        ""Periodo"": ""04/02/14 - 13/06/14"",
        ""LugaresHoras"": [
          {
            ""HoraInicio"": {
              ""ticks"": 684000000000
            },
            ""HoraFin"": {
              ""ticks"": 717000000000
            },
            ""Dias"": [
              1
            ],
            ""Edificio"": ""DUCT1"",
            ""Aula"": ""LC09""
          },
          {
            ""HoraInicio"": {
              ""ticks"": 684000000000
            },
            ""HoraFin"": {
              ""ticks"": 753000000000
            },
            ""Dias"": [
              3
            ],
            ""Edificio"": ""DUCT1"",
            ""Aula"": ""LC09""
          }
        ]
      },
      {
        ""CentroUniversitario"": ""D"",
        ""Maestro"": ""1700-18551700-1755 "",
        ""Seccion"": ""D58"",
        ""NRC"": ""06990"",
        ""Cupo"": 24,
        ""CupoDisponible"": 3,
        ""Periodo"": ""04/02/14 - 13/06/14"",
        ""LugaresHoras"": [
          {
            ""HoraInicio"": {
              ""ticks"": 612000000000
            },
            ""HoraFin"": {
              ""ticks"": 681000000000
            },
            ""Dias"": [
              1
            ],
            ""Edificio"": ""DUCT1"",
            ""Aula"": ""LC08""
          },
          {
            ""HoraInicio"": {
              ""ticks"": 612000000000
            },
            ""HoraFin"": {
              ""ticks"": 645000000000
            },
            ""Dias"": [
              3
            ],
            ""Edificio"": ""DUCT1"",
            ""Aula"": ""LC09""
          }
        ]
      },
      {
        ""CentroUniversitario"": ""D"",
        ""Maestro"": ""0900-09550900-1055 "",
        ""Seccion"": ""D60"",
        ""NRC"": ""06992"",
        ""Cupo"": 24,
        ""CupoDisponible"": 2,
        ""Periodo"": ""04/02/14 - 13/06/14"",
        ""LugaresHoras"": [
          {
            ""HoraInicio"": {
              ""ticks"": 324000000000
            },
            ""HoraFin"": {
              ""ticks"": 357000000000
            },
            ""Dias"": [
              2
            ],
            ""Edificio"": ""DUCT2"",
            ""Aula"": ""LC05""
          },
          {
            ""HoraInicio"": {
              ""ticks"": 324000000000
            },
            ""HoraFin"": {
              ""ticks"": 393000000000
            },
            ""Dias"": [
              4
            ],
            ""Edificio"": ""DUCT2"",
            ""Aula"": ""LC09""
          }
        ]
      },
      {
        ""CentroUniversitario"": ""D"",
        ""Maestro"": ""0700-08550700-0755 "",
        ""Seccion"": ""D62"",
        ""NRC"": ""06993"",
        ""Cupo"": 24,
        ""CupoDisponible"": 2,
        ""Periodo"": ""04/02/14 - 13/06/14"",
        ""LugaresHoras"": [
          {
            ""HoraInicio"": {
              ""ticks"": 252000000000
            },
            ""HoraFin"": {
              ""ticks"": 321000000000
            },
            ""Dias"": [
              1
            ],
            ""Edificio"": ""DUCT1"",
            ""Aula"": ""LC08""
          },
          {
            ""HoraInicio"": {
              ""ticks"": 252000000000
            },
            ""HoraFin"": {
              ""ticks"": 285000000000
            },
            ""Dias"": [
              3
            ],
            ""Edificio"": ""DEDU"",
            ""Aula"": ""A003""
          }
        ]
      },
      {
        ""CentroUniversitario"": ""H"",
        ""Maestro"": ""1400-1655"",
        ""Seccion"": ""H01"",
        ""NRC"": ""20240"",
        ""Cupo"": 20,
        ""CupoDisponible"": 9,
        ""Periodo"": ""04/02/14 - 13/06/14"",
        ""LugaresHoras"": [
          {
            ""HoraInicio"": {
              ""ticks"": 504000000000
            },
            ""HoraFin"": {
              ""ticks"": 609000000000
            },
            ""Dias"": [
              4
            ],
            ""Edificio"": ""HEDC"",
            ""Aula"": ""0010""
          }
        ]
      },
      {
        ""CentroUniversitario"": ""I"",
        ""Maestro"": ""1500-1655"",
        ""Seccion"": ""I01"",
        ""NRC"": ""21721"",
        ""Cupo"": 40,
        ""CupoDisponible"": 1,
        ""Periodo"": ""04/02/14 - 29/05/14"",
        ""LugaresHoras"": [
          {
            ""HoraInicio"": {
              ""ticks"": 540000000000
            },
            ""HoraFin"": {
              ""ticks"": 609000000000
            },
            ""Dias"": [
              2,
              4
            ],
            ""Edificio"": ""IEDG"",
            ""Aula"": ""0004""
          }
        ]
      }
    ]
  }
]";
        #endregion

        [TestMethod]
        public void EncuentraHorario()
        {
            var materias = JsonConvert.DeserializeObject<List<Models.Materia>>(data);

            Combinador c = new Combinador();
            var horarios = c.EncuentraTodos(materias, maxDuracionPorHueco: TimeSpan.MaxValue, soloConCupo: false);

            Assert.IsTrue(horarios.Count == 9135);
            Assert.IsTrue(horarios.Distinct().Count() == horarios.Count);
        }
    }
}
