Consider the following code for the view:

```fs
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
  ```
  
  When user presses the button, `Avalonia.FuncUI` fails internally.
  1st exception occurs when `patchProperty` function tries to set StackPanel's styles property to null:
  in Avalonia `Styles` of `StyledElement` are declared like this:
  
  ```cs
public Styles Styles
{
    get { return _styles ?? (Styles = new Styles()); }
    set
    {
        Contract.Requires<ArgumentNullException>(value != null);
        ...
    }
}
```

We can add setting styles to empty styles collection in `WithoutStyle` case:

```fs
StackPanel.styles (Styles())
```

But then application will start to fail when `null` is tried to be set as a `classes` property of a button as Avalonia doesn't support classes to be null either.
