using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mikulásbácsihozta_PDD.Models
{
    internal class HTMLController
    {
        public static void HTMLGeneralo(List<User> users)
        {     
            string tablazat = "";
            foreach (var user in users)
            {
                string fejkiemelo = (user.pillhely == 1) ? "table-figyelmezteto fofej" : "";
                string tablazatsor =
                    "\n<tr class=\"" + fejkiemelo + "\">" +
                    "\n<td style=\"width: 10%;\">" + user.pillhely + ".</td>" +
                    "\n<td style=\"width: 30%;\">" + user.Nev + "</td>" +
                    "\n<td style=\"width: 15%;\">" + user.pont1 + " (" + user.ido1 + "s)</td>" +
                    "\n<td style=\"width: 15%;\">" + user.pont2 + " (" + user.ido2 + "s)</td>" +
                    "\n<td style=\"width: 15%;\">" + user.pont3 + " (" + user.ido3 + "s)</td>" +
                    "\n<td style=\"width: 15%;\">" + user.Legjobbpont + " (" + user.Legjobbido + "s)</td>" +
                    "\n</tr>";
                tablazat += tablazatsor;
            }
            string htmlTemplate = $@"
        <!DOCTYPE html>
        <html lang=""hu"">
        <head>
            <meta charset=""UTF-8"">
            <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
            <title>Szélesbálási Fedettpályás Kalaplengető Verseny</title>
            <link href=""https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css"" rel=""stylesheet"">
            <link rel=""stylesheet"" href=""css/verseny.css"">
        </head>
        <body class=""d-flex flex-column min-vh-100"">

            <header>
                <div class=""fejlectartalom kozpont"">
                    <h1>Szélesbálási Kalaplengető Verseny</h1>
                    <p>Fedettpályás Bajnokság - Faluház Bálterme</p>
                </div>
            </header>

            <nav>
                <div class=""navkontener"">
                    <a href=""index.html"">Főoldal</a>
                    <a href=""verseny.html"" class=""aktiv"">Kalaplengető Verseny</a>
                </div>
            </nav>

        <div id=""fokezelo"" class=""container-fluid my-4 flex-grow-1 px-3"">
        
                <div class=""row h-100"">
                    <div class=""col-lg-2 col-md-3 d-flex flex-column gap-4 align-items-center justify-content-start oldalsavhatter p-3"">
                        <div class=""szimbolumkartya w-100"">
                            <img src=""img/cimer.png"" alt=""Traktoros címer búzakalásszal"" class=""img-fluid rounded border border-warning"">
                            <p class=""mt-2 fofej text-siker textkozep"">Falu Címere</p>
                        </div>
                        <div class=""szimbolumkartya w-100"">
                            <img src=""img/hurka.png"" alt=""Májas hurka"" class=""img-fluid rounded border border-warning"">
                            <p class=""mt-2 fofej text-siker textkozep"">Helyi Májas</p>
                        </div>
                    </div>

                    <div class=""col-lg-8 col-md-6 d-flex flex-column"">
                
                        <div class=""versenybevezeto textkozep mb-3 p-2"">
                            <h2>Aktuális Eredmények</h2>
                            <p class=""mb-0"">Legutóbbi frissítés: {DateTime.Now:yyyy.MM.dd. HH:mm:ss}</p>
                        </div>

                        <div class=""fejlectabla"">
                            <table class=""table mb-0 egyeditabla"">
                                <thead>
                                    <tr>
                                        <th style=width: 10%;>Hely.</th>
                                        <th style=width: 30%;>Versenyző Neve</th>
                                        <th style=width: 15%;>1. Kör (Pont/Idő)</th>
                                        <th style=width: 15%;>2. Kör (Pont/Idő)</th>
                                        <th style=width: 15%;>3. Kör (Pont/Idő)</th>
                                        <th style=width: 15%;>Legjobb (Pont/Idő)</th>
                                    </tr>
                                </thead>
                            </table>
                        </div>

                        <div class=""eredmenyablak flex-grow-1 border-start border-end border-bottom rounded-bottom arnyek"" id=""gorgetokontener"">
                            <div id=""gorgetotartalom"">
                                <table class=""table table-striped table-hover mb-0 egyeditabla"">
                                    <tbody>
                                        {tablazat}
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>

                    <div class=""col-lg-2 col-md-3 d-flex flex-column align-items-center justify-content-start oldalsavhatter p-3"">
                        <div class=""szimbolumkartya w-100"">
                            <img src=""img/sor.png"" alt=""Teli söröskorsó"" class=""img-fluid rounded border border-warning"">
                            <p class=""mt-2 fofej text-siker textkozep"">A Fődíj</p>
                        </div>
                    </div>
                </div>
            </div>
            <script>
                const tabla = document.getElementById('gorgetokontener');
                const tartalom = document.getElementById('gorgetotartalom');
                let poz = 0;
                const sebesseg = 0.5;
        
                const Loop = () => {{
                    if (tartalom) {{
                         poz += sebesseg;
                         if (poz >= tartalom.offsetHeight) poz = 0;
                         tabla.scrollTop = poz;
                         requestAnimationFrame(Loop);
                    }}
                }};

                window.onload = () => {{
                     const innerTable = tartalom.querySelector('table'); 
                     if (innerTable.offsetHeight > tabla.offsetHeight) {{
                         tabla.appendChild(tartalom.cloneNode(true));
                         Loop();
                     }}
                }};
            </script>
            <footer>
                <p>Szélesbálás Község Önkormányzata | 1234 Szélesbálás, Arató utca 321.</p>
                <p class=""small text-muted"">Hivatalos időmérés: Jegyzői Okosóra (századmásodperc pontosság).</p>
            </footer>
        </body>
        </html>";
        File.WriteAllText("verseny.html", htmlTemplate, System.Text.Encoding.UTF8);
        }
    }
}
