   M S B A M L   `   `   `  ����A  =FastDB, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:FastDB.ClassFastDB.Class  5      /clr-namespace:FastDB.ControlFastDB.Control  5      +    %FastDB.Control.ScriptGeneratorControl   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35I EWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=nullH 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      ,localclr-namespace:FastDB.Class     5      2	myControlclr-namespace:FastDB.Control     5        � 	Resources  5
       �� Source�� 5   
   $ ../theme/generic.xaml?�6   %    5      .��5      � $	��600��6   $
��Left=�6   .+�5   
   ]� -   5        $��DockFromTab��5      $
��Auto��6'   $
��Auto��65   $
��Left=�6B   .+�5      ]� $
��Auto��6   $
��Auto��6&   $
��Left=�63   .+�5      �� -   5        $��StackPanelTopControl2��5      $	��Top=�6:   .+�5      �� $��5,0,0,5q�6    �� Padding$ 3q�6.   
�������
�����F�$��0,0,0,0q�6j    �� CornerRadius$ 0��6�   $	��Top=�5      $��39��65   $	��518��6A   $
��Left=�6M   ._�5      �� -   5        $��StackPanelTopControl3��5   %   $	��Top=�6B   $	��511��6W   $	��0,0q�6c   $��31��6p   .+�5      �� -   5        $��	btnSubmit��5   %   $��Submit��66   $��0,0,5,0q�6G   $��1��6X   $��27��6}   $��81��6�      buttonStyle#��[  6�   $��Right=�6�   6�   5      5      5      �� 5      -   5        $��StackPanelTopControl1��5      $	��Top=�6:   .+�5      �� $��5,5,0,0q�6   $ 1q�6.   
�������
�����F�$��1,1,1,1q�6j   $ 0��6�   $	��Top=�5      $
��Auto��65   $	��518��6C   $
��Left=�6O   ._�5      �� -   5        $��StackPanelTopControl��5   %   $	��Top=�6A   $	��517��6V   $	��0,0q�6b   $��27��6o   .+�5      ]� -   5        $��DockFromTabTopControl��5   (   $	��0,0q�6F   $
��Left=�6S   .+�5    "   �� -   5        $��btnAdd��5    )   $��
Add Column��67   $��0,0,5,0q�6L   $��81��6r   $��Right=�6}   $��1��6�   $��27��6�   #��[  5!   )   6P   5(      5)      5*      5+      ]� 5,      -	   5        $��borderJoinDock��5,      $
��Left=�62   $y�Hidden=�6N   $��0,0,0,0q�6b   $
��Auto��6s   $	��558��6�   .+�5-      �� $
��Auto=�6#   $��Disabled=�6G   .��5.      �� -
   5        $��
borderJoin��5.   !   $	��0,5q�63   $ 3q�6@   
�������
�����F�$��1,1,1,1q�5/   !   $ 0��6;   $	��Top=�6L   $
��Auto��6d   $	��524��6r   $
��Left=�50   !   ._�51      �� $	��Top=�6)   $
��Left=�6>   $	��516��6Y   $
��Auto��6e   .+�52   "   �� $	��Top=�6-   $
��Left=�6B   $��0,0,0,0q�6]   .+�53   &   ]� -   5        $��DockPanelFromTabRowHeader��53   0   $
��Left=�6Q   $y�Hidden=�54   0   $��0,0,0,0q�6D   $��20��6U   .+�55   *   �� $��Column Name��60   $��90��6F   $��Center=�6Q   $��	30,0,10,0q�6n   $��Calibri)�6�   $��Normal"�6�   $��14.667(�6�   �� 56   *   $��DataType��60   $��60��6C   $
��25,0q�6N   $��Center=�6\   $��Calibri)�6y   $��Normal"�6�   $��14.667(�6�   �� 57   *   $
��Size��60   $��42��6?   $��Center=�6J   $��20,0,5,0q�6g   $��Calibri)�6y   $��Normal"�6�   $��14.667(�6�   �� 58   *   $��Look Up��60   $��60��6B   $��Center=�6M   $��0,0,15,0q�6j   $��Calibri)�6|   $��Normal"�6�   $��14.667(�6�   �� 59   *   $��IsNULL��60   $��50��6A   $��Center=�6L   $��0,0,5,0q�6i   $��Calibri)�6z   $��Normal"�6�   $��14.667(�6�   5:   '   5;   #   ]� 5<   "   $
��Auto��6,   .+�5=   &   �� -   5        $��StackPanelScriptGen��5=   G   $	��Top=�61   $��Stretch=�6b    �� CanVerticallyScroll .  �� CanHorizontallyScroll . $
��Left=�5>   N   $��8,0,0,0q�6i   5?   '   5@   #   ]� 5A   "   $
��Auto��6,   $	��515��6:   .+�5B   &   �� $	��Top=�61   $��Stretch=�6G    .  . $��Right=�5C   N   $��
420,10,0,0q�6j   5D   '   �� 5E   &   -   5        $��btnGenerateScript��5E   G   $��Generate Script��6-   $	��102��6`   $��Right=�6�   $��27��5F   -   #��[  69   $��0,0,5,0q�6^   5G   '   5H   #   5I      5J      5K      5L      5M      5N      5O      5P      