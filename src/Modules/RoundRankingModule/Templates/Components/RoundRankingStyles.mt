<component>
    <import component="EvoSC.Style.UIStyle" as="UIStyle"/>
    
    <template>
        <UIStyle/>
        <stylesheet>
            <style
                    class="lr-body-primary"
                    bgcolor="{{ Theme.UI_RoundRankingModule_Widget_Row_Bg }}"
                    opacity="{{ Theme.UI_LocalRecordsModule_Widget_Row_Bg_Opacity }}"
            />
            <style
                    class="lr-body-highlight"
                    bgcolor="{{ Theme.UI_RoundRankingModule_Widget_Row_Bg_Highlight }}"
                    opacity="{{ Theme.UI_LocalRecordsModule_Widget_Row_Bg_Opacity }}"
            />
        </stylesheet>
    </template>
</component>