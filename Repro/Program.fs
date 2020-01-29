namespace Repro

open Avalonia
open Avalonia.FuncUI.DSL
open Elmish
open Avalonia.FuncUI.Components.Hosts
open Avalonia.FuncUI
open Avalonia.FuncUI.Elmish
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.Styling
open Avalonia.Controls
open Avalonia.Media

type Model =
    | WithStyle
    | WithoutStyle
    
type Msg =
    | Switch
    
module Core =
    let init = WithStyle
    
    let view model dispatch =
        let styles =
            let styles = Styles()
            
            let fieldStyle = Style(fun x -> x.Class("class"))
            fieldStyle.Setters.Add (Setter(TextBox.BorderBrushProperty, Brushes.Red))
            styles.Add fieldStyle
            
            styles
        
        match model with
        | WithStyle ->
            StackPanel.create [
                StackPanel.styles styles
                StackPanel.children [
                    Button.create [
                        Button.classes ["class"]
                        Button.content "Remove style"
                        Button.onClick (fun _ -> Switch |> dispatch)
                    ]
                ]
            ]
        | WithoutStyle ->
            StackPanel.create [
//                StackPanel.styles (Styles())
                StackPanel.children [
                    Button.create [
//                        Button.classes []
                        Button.content "Add style"
                        Button.onClick (fun _ -> Switch |> dispatch)
                    ]
                ]
            ]
            
    let update _msg model =
        match model with
        | WithStyle ->
            WithoutStyle, Cmd.none
        | WithoutStyle ->
            WithStyle, Cmd.none

type MainWindow() as this =
    inherit HostWindow()
    do
        base.Title <- "Repro"
        base.Width <- 200.
        base.Height <- 200.
        
        let state = fun () -> Core.init, Cmd.none
        
        Elmish.Program.mkProgram state Core.update Core.view
        |> Program.withHost this
        |> Program.run
        
type App() =
    inherit Application()

    override this.Initialize() =
        this.Styles.Load "resm:Avalonia.Themes.Default.DefaultTheme.xaml?assembly=Avalonia.Themes.Default"
        this.Styles.Load "resm:Avalonia.Themes.Default.Accents.BaseLight.xaml?assembly=Avalonia.Themes.Default"

    override this.OnFrameworkInitializationCompleted() =
        match this.ApplicationLifetime with
        | :? IClassicDesktopStyleApplicationLifetime as desktopLifetime ->
            let mainWindow = MainWindow()
            desktopLifetime.MainWindow <- mainWindow
        | _ -> ()

module Program =
    [<EntryPoint>]
    let main(args: string[]) =
        AppBuilder
            .Configure<App>()
            .UsePlatformDetect()
            .UseSkia()
            .StartWithClassicDesktopLifetime(args)